using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PersonalSettingsController : MonoBehaviour
{
    [SerializeField]
    private GameObject brightness;

    [SerializeField]
    private GameObject fxVolume;

    [SerializeField]
    private GameObject musicVolume;


    #region ButtonFunctions

    public void OnBrightnessAddClicked()
    {
        TextMeshProUGUI brightnessText = brightness.transform.Find("BrightnessText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(brightnessText.text, out int number);
        if (isInt)
        {
            if (number < 10)
            {
                number++;
                brightnessText.text = number.ToString();
            }
        }
    }

    public void OnBrightnessMinusClicked()
    {
        TextMeshProUGUI brightnessText = brightness.transform.Find("BrightnessText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(brightnessText.text, out int number);
        if (isInt)
        {
            if (number > 1)
            {
                number--;
                brightnessText.text = number.ToString();
            }
        }
    }

    public void OnFXAddClicked()
    {
        TextMeshProUGUI fxText = fxVolume.transform.Find("FXVolumeText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(fxText.text, out int number);
        if (isInt)
        {
            if (number < 10)
            {
                number++;
                fxText.text = number.ToString();
            }
        }
    }

    public void OnFXMinusClicked()
    {
        TextMeshProUGUI fxText = fxVolume.transform.Find("FXVolumeText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(fxText.text, out int number);
        if (isInt)
        {
            if (number > 1)
            {
                number--;
                fxText.text = number.ToString();
            }
        }
    }

    public void OnMusicAddClicked()
    {
        TextMeshProUGUI musicText = musicVolume.transform.Find("MusicVolumeText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(musicText.text, out int number);
        if (isInt)
        {
            if (number < 10)
            {
                number++;
                musicText.text = number.ToString();
            }
        }
    }

    public void OnMusicMinusClicked()
    {
        TextMeshProUGUI musicText = musicVolume.transform.Find("MusicVolumeText/Text").GetComponent<TextMeshProUGUI>();
        bool isInt = int.TryParse(musicText.text, out int number);
        if (isInt)
        {
            if (number > 1)
            {
                number--;
                musicText.text = number.ToString();
            }
        }
    }

    #endregion
}
