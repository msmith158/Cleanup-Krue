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
    public UnityEvent OnInteract;
    [SerializeField] private Custom2DController player;
    [SerializeField] private TextMeshPro interactionText;
    private bool withinRange = false;
    private bool isDialogueActivated = false;

    private void Start()
    {
        interactionText.enabled = false;
        OnInteract ??= new UnityEvent(); // "??=" is shorthand for a null check and will execute if the variable is null
    }

    private void OnEnable()
    {
        DialogueTransitions.InteractionFieldReactivate += ReactivateField;
    }

    private void OnDisable()
    {
        DialogueTransitions.InteractionFieldReactivate -= ReactivateField;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActivated && withinRange)
            {
                OnInteract.Invoke();
                player.canMove = false;
                isDialogueActivated = true;
            }
        }
    }

    private void ReactivateField()
    {
        isDialogueActivated = false;
        player.canMove = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Custom2DController>())
        {
            interactionText.enabled = true;
            withinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Custom2DController>())
        {
            interactionText.enabled = false;
            withinRange = false;
        }
    }
}
