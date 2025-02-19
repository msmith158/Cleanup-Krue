using System;
using System.Collections;
using System.Collections.Generic;
using Mitchel.UISystems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TestInteractionField : MonoBehaviour
{
    public List<TextAsset> TextAssets = new List<TextAsset>();
    public UnityEvent OnInteract;
    [SerializeField] private TestController player;
    [SerializeField] private TextMeshPro interactionText;
    private bool withinRange = false;
    private bool isDialogueActivated = false;

    private void Start()
    {
        interactionText.enabled = false;

        if (OnInteract == null)
            OnInteract = new UnityEvent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActivated && withinRange)
            {
                OnInteract.Invoke();
                DialogueTransitions.Instance.InitiateDialogue();
                player.CanMove = false;
                isDialogueActivated = true;
            }
            else if (isDialogueActivated && DialogueTransitions.Instance.ReadyToProceed)
            {
                DialogueTransitions.Instance.ExitDialogue();
                player.CanMove = true;
                isDialogueActivated = false;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TestController>())
        {
            interactionText.enabled = true;
            withinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TestController>())
        {
            interactionText.enabled = false;
            withinRange = false;
        }
    }
}
