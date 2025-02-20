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

    [Header("Audio Settings")] 
    public AudioClip DialogueCharSfx;
    public AudioClip DialogueProceedSfx;

    [Header("Object References")] 
    [SerializeField] private DialogueTransitions dialogueTransitions;
    [SerializeField] private TextMeshProUGUI dialogueHeader;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialoguePromptImage;
    
    // =============== Private value variables ===============
    private bool isPrinting;
    
    // =========== Private object reference variables ===========
    private List<string> dialogueLines;
    
    /// <summary>
    /// Saves the contents of the passed-through TextAsset to a list and begins the transition for the dialogue graphics.
    /// If you want to initialise the dialogue system, only call this function.
    /// </summary>
    /// <param name="dialogueBundle">The text file to be read from and printed to the dialogue system.</param>
    public void InitiateDialogue(TextAsset dialogueBundle)
    {
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
        
    }

    private IEnumerator StartDialoguePrinting()
    {
        isPrinting = true;
        dialogueText.text = "";
        dialogueHeader.text = "";
        //dialoguePromptImage.enabled = false;

        foreach (string s in dialogueLines)
        {
            Debug.Log(s);
        }
        
        yield return null;
    }
}
