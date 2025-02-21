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
    [SerializeField] private DialogueTransitions dialogueTransitions;
    [SerializeField] private TextMeshProUGUI dialogueHeader;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialoguePromptImage;
    [SerializeField] private AudioSource dialogueSfxSource;
    
    // =============== Private value variables ===============
    private bool isPrinting;
    private bool skipCheck;
    private bool dialogueSkipped = false;
    private bool dialogueEngaged = false;
    private int lineIteration = 0;
    
    // =========== Private object reference variables ===========
    private List<string> dialogueLines;
    
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

    private IEnumerator StartDialoguePrinting()
    {
        isPrinting = true;
        dialogueText.text = "";
        //dialogueHeader.text = "";
        dialoguePromptImage.enabled = false;

        dialogueSfxSource.clip = DialogueCharSfx;
        if (isFixedSfxTiming)
            StartCoroutine(PlaySFXFixed());

        int charIteration = 0; //
        for (int i = 0; i < dialogueLines[lineIteration].Length; i++)
        {
            char c = dialogueLines[lineIteration][i];

            if (c == '<')
            {
                Debug.Log("Detected formatting argument");
                string argument = "";
                int j = i; // Local iteration variable for iteration until a closing formatting bracket is detected
                while (c != '>')
                {
                    argument += c;
                    c = dialogueLines[lineIteration][++j];
                    Debug.Log("Moving to next character");
                }
                argument += '>';
                dialogueText.text += argument;
                c = dialogueLines[lineIteration][j];
            }
            
            dialogueText.text += c;
            
            if (!isFixedSfxTiming) 
                dialogueSfxSource.Play();
            if (c == '.' && pauseAtFullStop && i != dialogueLines[lineIteration].Length - 1) 
                yield return new WaitForSeconds(fullStopPauseTime);
            yield return new WaitForSeconds(charDelayTime);
            if (skipCheck)
            {
                dialogueText.text = dialogueLines[lineIteration]; // TODO: Set this up properly once inline argument parsing is implemented
                dialogueSkipped = true;
                break;
            }
        }
        
        foreach (char c in dialogueLines[lineIteration])
        {
            // Catching formatting arguments and instantly inserting them so the end user doesn't see it when they shouldn't.
            /*if (c == '<')
            {
                Debug.Log("Detected formatting argument");
                string argument = "";
                while (c != '>')
                {
                    argument += c;
                    charIteration++;
                    Debug.Log("Moving to next character");
                    continue;
                }
                argument += '>';
                dialogueText.text += argument;
                charIteration++;
            }*/
            
            dialogueText.text += c;
            
            if (!isFixedSfxTiming) 
                dialogueSfxSource.Play();
            if (c == '.' && pauseAtFullStop && charIteration != dialogueLines[lineIteration].Length - 1) 
                yield return new WaitForSeconds(fullStopPauseTime);
            yield return new WaitForSeconds(charDelayTime);
            if (skipCheck)
            {
                dialogueText.text = dialogueLines[lineIteration]; // TODO: Set this up properly once inline argument parsing is implemented
                dialogueSkipped = true;
                break;
            }
            charIteration++;
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