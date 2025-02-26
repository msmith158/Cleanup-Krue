using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class SwingingDoors : MonoBehaviour
{
    public Animator animatorleft;
    public Animator animatorright;
    public string doorOpenEventName = "DoorOpen";
    //public string doorCloseEventName = "DoorClose";
    public float doorCloseDelay = 3f;
    public GameObject objectToEnable;
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
            if (IsPlayerFacingDoor() && !isDoorOpen) // Check if the door is closed here
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
        animatorleft.SetBool("Door open", true);
        animatorright.SetBool("Door open", true);
        AkSoundEngine.PostEvent(doorOpenEventName, gameObject);
    }

    private void CloseDoor()
    {
        animatorleft.SetBool("Door open", false);
        animatorright.SetBool("Door open", false);
    }

    private void EnableObject()
    {
        if (objectToEnable != null && !isObjectEnabled)
        {
            objectToEnable.SetActive(true);
            isObjectEnabled = true;
        }
    }

    private void DisableObject()
    {
        if (objectToEnable != null && isObjectEnabled)
        {
            objectToEnable.SetActive(false);
            isObjectEnabled = false;
        }
    }
}

