using System;
using System.Collections;
using System.Collections.Generic;
using Mitchel.UISystems;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TestInteractionField : MonoBehaviour
{
    [SerializeField] private TestController player;
    [SerializeField] private DialogueSystem dialogueSys;
    [SerializeField] private TextMeshPro interactionText;
    private bool withinRange = false;
    private bool isDialogueActivated = false;

    private void Start()
    {
        interactionText.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActivated && withinRange)
            {
                dialogueSys.InitiateDialogue();
                player.CanMove = false;
                isDialogueActivated = true;
            }
            else
            {
                dialogueSys.ExitDialogue();
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
