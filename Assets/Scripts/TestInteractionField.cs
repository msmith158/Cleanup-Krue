using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TestInteractionField : MonoBehaviour
{
    [SerializeField] private TestDialogueSystem dialogueSys;
    [SerializeField] private TextMeshPro interactionText;
    private bool withinRange = false;

    private void Start()
    {
        interactionText.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && withinRange)
        {
            dialogueSys.StartDialogue();
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
