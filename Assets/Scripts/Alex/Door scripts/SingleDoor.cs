using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class SingleDoor : MonoBehaviour
{
    public Animator animator; 
    public string doorOpenEventName = "Play_Door_Open";
    public float doorCloseDelay = 3f;
    public GameObject enableUI; 
    public Transform playerTransform; 

    private bool isDoorOpen = false;
    private float doorCloseTime;
    private bool isPlayerInTrigger = false;

    private static bool isObjectEnabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            DisableObject();
            if (isDoorOpen)
            {
                doorCloseTime = Time.time + doorCloseDelay;
            }
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            if (IsPlayerFacingDoor() && !isDoorOpen)
            {
                EnableObject();
            }
            else
            {
                DisableObject();
            }
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E) && IsPlayerFacingDoor())
        {
            if (!isDoorOpen)
            {
                OpenDoor();
                isDoorOpen = true;
                doorCloseTime = float.PositiveInfinity;
                DisableObject();
            }
        }

        if (isDoorOpen && Time.time >= doorCloseTime)
        {
            CloseDoor();
            isDoorOpen = false;
        }
    }

    private bool IsPlayerFacingDoor()
    {
        Vector3 directionToDoor = (transform.position - playerTransform.position).normalized;
        float dot = Vector3.Dot(playerTransform.forward, directionToDoor);
        return dot > 0; // Adjust this threshold as needed
    }

    private void OpenDoor()
    {
        animator.SetBool("Door open", true);
        AkSoundEngine.PostEvent(doorOpenEventName, gameObject);
    }

    private void CloseDoor()
    {
        animator.SetBool("Door open", false);
    }

    private void EnableObject()
    {
        if (enableUI != null && !isObjectEnabled)
        {
            enableUI.SetActive(true);
            isObjectEnabled = true;
            //Debug.Log("ENABLED TRIGGER OCCURRED!!!");
        }
    }

    private void DisableObject()
    {
        if (enableUI != null && isObjectEnabled)
        {
            enableUI.SetActive(false);
            isObjectEnabled = false;
            //Debug.Log("DISABLED TRIGGER OCCURRED!!!");
        }
    }
}
