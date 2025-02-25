using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mitchel.UISystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("General Settings")] 
    [SerializeField] private float charDelayTime;
    [SerializeField] private bool pauseAtFullStop;
    [SerializeField] private float fullStopPauseTime;

    [Header("Audio Settings")] 
    public AudioClip DialogueCharSfx;
    public AudioClip DialogueProceedSfx;
    [SerializeField] private bool isFixedSfxTiming;
    [SerializeField] private float fixedSfxTiming;

    [Header("Object References")]
    public Image dialogueCharacterImage;
    public TextMeshProUGUI dialogueHeader;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialoguePromptImage;
    [SerializeField] private AudioSource dialogueSfxSource;
    
    // =============== Private value variables ===============
    private bool isPrinting;
    private bool skipCheck;
    private bool dialogueSkipped = false;
    private bool dialogueEngaged = false;
    private int lineIteration = 0;
    private int lineCharIndex = 0;
    
    // =========== Private object reference variables ===========
    [SerializeField] private DialogueUtils dialogueUtils;
    [SerializeField] private DialogueTransitions dialogueTransitions;
    private List<string> dialogueLines;

    private void Start()
    {
        dialogueUtils = GetComponent<DialogueUtils>();
        dialogueTransitions = GetComponent<DialogueTransitions>();
    }

    /// <summary>
    /// Saves the contents of the passed-through TextAsset to a list and begins the transition for the dialogue graphics.
    /// If you want to initialise the dialogue system, only call this function.
    /// </summary>
    /// <param name="dialogueBundle">The text file to be read from and printed to the dialogue system.</param>
    public void InitiateDialogue(TextAsset dialogueBundle)
    {
        dialogueEngaged = true;
        dialogueText.text = "";
        //dialogueHeader.text = "";
        dialoguePromptImage.enabled = false;
        
        if (dialogueLines != null) dialogueLines.Clear();
        dialogueLines = dialogueBundle.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList(); // This splits each line into a string array and then assigns it to the list.
        lineIteration = 0;
        
        CheckInlineArguments();
        dialogueTransitions.EnterDialogue();
    }
    
    /// <summary>
    /// Starts printing the dialogue to the dialogue box from a list of lines saved from the TextAsset passed through from the InitiateDialogue function.
    /// To initialise the dialogue system, call InitiateDialogue() instead.
    /// </summary>
    public void PrintDialogue()
    {
        StartCoroutine(StartDialoguePrinting());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPrinting && dialogueEngaged) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialogueEngaged) 
                {
                    dialogueSfxSource.PlayOneShot(DialogueProceedSfx);
                    if (lineIteration >= dialogueLines.Count)
                        dialogueEngaged = false;
                }

                if (lineIteration < dialogueLines.Count)
                    StartCoroutine(StartDialoguePrinting());
                else if (lineIteration >= dialogueLines.Count)
                {
                    dialogueEngaged = false;
                    dialogueText.text = "";
                    dialogueTransitions.ExitDialogue();
                }
            }
        }
        else if (isPrinting)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                skipCheck = true;
                Debug.Log("Skipped dialogue");
            }
        }
    }

    private void CheckInlineArguments()
    {
        int minCharIndex = lineCharIndex;
        // Check inline argument the first time.
        if (dialogueLines[lineIteration][lineCharIndex] == '[')
        {
            string inlineTag = "";
            while (dialogueLines[lineIteration][lineCharIndex] != ']')
            {
                inlineTag += dialogueLines[lineIteration][lineCharIndex];
                lineCharIndex++;
            }
            inlineTag += ']';
            lineCharIndex++;
            dialogueUtils.ProcessInlineArgument(inlineTag);
            dialogueLines[lineIteration] =
                dialogueLines[lineIteration].Remove(minCharIndex, lineCharIndex - minCharIndex);
            lineCharIndex = 0;
        }

        // If another inline argument comes right after, re-run the function.
        if (dialogueLines[lineIteration][lineCharIndex] == '[')
        {
            Debug.Log("Re-running inline arguments check");
            CheckInlineArguments();
        }
        
        Debug.Log("Inline arguments check complete");
    }

    private IEnumerator StartDialoguePrinting()
    {
        lineCharIndex = 0;
        CheckInlineArguments();

        isPrinting = true;
        dialogueText.text = "";
        dialoguePromptImage.enabled = false;
        
        // Setting up the sound effect clip and timing for character printing.
        dialogueSfxSource.clip = DialogueCharSfx;
        if (isFixedSfxTiming)
            StartCoroutine(PlaySFXFixed());

        // This is the actual dialogue printing code
        for (int i = 0; i < dialogueLines[lineIteration].Length; i++)
        {
            char c = dialogueLines[lineIteration][i];

            // Detect formatting tags and insert them immediately so they don't get printed
            if (c == '<')
            {
                string argument = "";
                int j = i; // Local iteration variable for iteration until a closing formatting bracket is detected
                while (c != '>')
                {
                    argument += c;
                    c = dialogueLines[lineIteration][++j];
                }
                
                dialogueText.text += argument;
                i = j;
                c = dialogueLines[lineIteration][i];
            }
            // Check for an inline argument in the middle of a line, e.g. for a text effect
            else if (c == '[')
            {
                lineCharIndex = i;
                CheckInlineArguments();
                i += lineCharIndex;
            }
            
            dialogueText.text += c;
            
            if (!isFixedSfxTiming) 
                dialogueSfxSource.Play();
            if (c == '.' && pauseAtFullStop && (i != dialogueLines[lineIteration].Length - 1 || dialogueLines[lineIteration][i] != '<')) 
                yield return new WaitForSeconds(fullStopPauseTime);
            yield return new WaitForSeconds(charDelayTime);
            if (skipCheck)
            {
                dialogueText.text = dialogueLines[lineIteration]; // TODO: Set this up properly once inline argument parsing is implemented
                dialogueSkipped = true;
                break;
            }
        }

        dialoguePromptImage.enabled = true;
        isPrinting = false;
        skipCheck = false;
        lineIteration++;
    }

    private IEnumerator PlaySFXFixed()
    {
        while (isPrinting)
        {
            dialogueSfxSource.Play();
            yield return new WaitForSeconds(fixedSfxTiming);
        }
    }
}