using System;
using System.Collections;
using System.Collections.Generic;
using Mitchel.UISystems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
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
                player.CanMove = false;
                isDialogueActivated = true;
            }
        }
    }

    private void ReactivateField()
    {
        isDialogueActivated = false;
        player.CanMove = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<TestController>())
        {
            interactionText.enabled = true;
            withinRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<TestController>())
        {
            interactionText.enabled = false;
            withinRange = false;
        }
    }
}
