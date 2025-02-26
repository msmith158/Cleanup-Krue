using UnityEngine;
using UnityEngine.UI;
using AK.Wwise;
using System.Collections;

public class ChangeVolumeLevels : MonoBehaviour
{
    public Slider thisSlider;

    public void SetVolume(string whatValue)
    {
        float sliderValue = thisSlider.value;

        if (whatValue == "SFX")
            AkSoundEngine.SetRTPCValue("Bus_Volume_SFX", sliderValue);
        
        if (whatValue == "Music")
            AkSoundEngine.SetRTPCValue("Bus_Volume_Music", sliderValue);

        if (whatValue == "Ambience")
            AkSoundEngine.SetRTPCValue("Bus_Volume_Ambience", sliderValue);

        if (whatValue == "Footsteps")
            AkSoundEngine.SetRTPCValue("Bus_Volume_Footsteps", sliderValue);

        if (whatValue == "Aux_Reverb_1")
            AkSoundEngine.SetRTPCValue("Aux_Reverb_1", sliderValue);

        if (whatValue == "Aux_Reverb_2")
            AkSoundEngine.SetRTPCValue("Aux_Reverb_2", sliderValue);

        if (whatValue == "Aux_Reverb_3")
            AkSoundEngine.SetRTPCValue("Aux_Reverb_3", sliderValue);
    }
}