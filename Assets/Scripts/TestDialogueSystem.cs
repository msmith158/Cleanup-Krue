using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestDialogueSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private string textString;
    [SerializeField] private float timerLength;
    [HideInInspector] public bool IsRunning = false;
    
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Start()
    {
        dialogueText.enabled = false;
    }
    
    public void StartDialogue()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            StartCoroutine(RunDialogueTimer());
        }
    }

    private IEnumerator RunDialogueTimer()
    {
        dialogueText.text = textString;
        dialogueText.enabled = true;

        yield return new WaitForSeconds(timerLength);

        dialogueText.text = "";
        dialogueText.enabled = false;
        IsRunning = false;
    }
}
