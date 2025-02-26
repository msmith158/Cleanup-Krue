using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSwapper : MonoBehaviour
{
  
    public void CheckLayers()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            if (hit.transform.GetComponent<SurfaceType>() != null)
            {
                string surfaceType = hit.transform.GetComponent<SurfaceType>().surfaceType;
                AkSoundEngine.SetSwitch("SurfaceType", surfaceType, gameObject);
            }
        }
    }
}