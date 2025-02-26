using UnityEngine;
using UnityEngine.UI;
using AK.Wwise;

public class SFXVolumeControl : MonoBehaviour
{
    public string sfxVolumeRtpcName = "SFXVolume"; // Name of the RTPC in Wwise
    public Slider sfxVolumeSlider; // Reference to the Slider UI element

    private RTPC sfxVolumeRtpc; // Reference to the RTPC object in Wwise

    private void Start()
    {
        // Get the RTPC object from Wwise
        sfxVolumeRtpc = new RTPC();
        sfxVolumeRtpc.SetGlobalValue(sfxVolumeSlider.value);

        // Add a listener to the Slider's value change event
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Method to set the SFX volume based on the Slider's value
    public void SetSFXVolume(float volume)
    {
        if (sfxVolumeRtpc != null)
        {
            Debug.Log(volume);
            sfxVolumeRtpc.SetGlobalValue(volume);
        }
    }
}