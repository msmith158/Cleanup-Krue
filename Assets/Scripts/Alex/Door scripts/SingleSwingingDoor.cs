using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSwingingDoor : MonoBehaviour
{

    public Animator animatorVault;
    public string doorOpenEventName = "DoorOpen";
    public string doorCloseEventName = "DoorClose";


    private void OnTriggerEnter(Collider other)
    {
        animatorVault.SetBool("Door open", true);
        AkSoundEngine.PostEvent(doorOpenEventName, gameObject);
    }


    private void OnTriggerExit(Collider other)
    {
        animatorVault.SetBool("Door open", false);
        AkSoundEngine.PostEvent(doorCloseEventName, gameObject);
    }
}