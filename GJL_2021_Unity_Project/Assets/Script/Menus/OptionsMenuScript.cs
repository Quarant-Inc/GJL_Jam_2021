using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{

    public AK.Wwise.Event clickSound = null;
    public Button returnButton;
    Slider musicVolumeSlider;
    Slider sFXVolumeSlider;
    float startingMusicVol;
    float startingSFXVol;

    [SerializeField]
    private AK.Wwise.RTPC rtpcMusic = null;

    [SerializeField]
    private AK.Wwise.RTPC rtpcFX = null;

    // Start is called before the first frame update
    void Start()
    {
        musicVolumeSlider.value = rtpcMusic.GetGlobalValue();
        sFXVolumeSlider.value = rtpcFX.GetGlobalValue();
        //musicMyFill.fillAmount = current_options.music_vol / 10;
        //sFXMyFill.fillAmount = current_options.sfx_vol / 10;
        ;
    }

    // Update is called once per frame
    void Update()
    {
        //current_options.music_vol = musicVolumeSlider.value;
        //current_options.sfx_vol = sFXVolumeSlider.value;
        AkSoundEngine.SetRTPCValue("MusicVolume", musicVolumeSlider.value);
        AkSoundEngine.SetRTPCValue("SFXVolume", sFXVolumeSlider.value);
    }


    public void ReturnToMenu()
    {
        returnButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        StartCoroutine(DelayReturnToMenu());
    }

    IEnumerator DelayReturnToMenu()
    {
        yield return new WaitForSeconds(0.04f);
        returnButton.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
        SceneManager.LoadScene("MainMenu");

    }

}
