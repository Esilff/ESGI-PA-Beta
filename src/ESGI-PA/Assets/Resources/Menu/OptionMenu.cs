using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] bool keyboard;
    [SerializeField] KeyCode[] tabKeyCodes;
    [SerializeField] Image[] images;
    [SerializeField] Text[] texts;
    [SerializeField] int index;
    [SerializeField] Image btnsImage;
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Resolution[] resolutions;
    [SerializeField] Toggle fsToggle;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider smainv;
    [SerializeField] Slider smusicv;
    [SerializeField] Slider seffectv;
    
    void Awake()
    {
        resolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        bool notExist = true;
        System.Collections.Generic.List<Resolution> resols = new System.Collections.Generic.List<Resolution>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            notExist = true;

            foreach (var o in resolutionDropdown.options)
                if (Screen.resolutions[i].width + "x" + Screen.resolutions[i].height == o.text)
                    notExist = false;

            if (notExist)
            {
                resolutionDropdown.options.Add(new Dropdown.OptionData(Screen.resolutions[i].width + "x" + Screen.resolutions[i].height));
                resols.Add(Screen.resolutions[i]);
            }

            if (Screen.resolutions[i].width == Screen.currentResolution.width && Screen.resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutions = resols.ToArray();
        OptionsSave os = SaveScript.LoadOptionsSaves();
        if (os == null)
        {
            os = new OptionsSave(tabKeyCodes, new float[] {20, 20, 20}, new float[] {1, 1, 1}, currentResolutionIndex, true);
            SaveScript.SaveOptions(os.tabKeyCodes, os.volumes, os.slidersVolumes, os.indexRes, os.isFullScreen);
        }

        tabKeyCodes = os.tabKeyCodes;
        currentResolutionIndex = os.indexRes;
        Screen.fullScreen = os.isFullScreen;
        fsToggle.isOn = os.isFullScreen;

        smainv.value = os.slidersVolumes[0];
        smusicv.value = os.slidersVolumes[1];
        seffectv.value = os.slidersVolumes[2];

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = tabKeyCodes[i].ToString();
        }
    }
    
    void Update()
    {
        if (keyboard)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    for (int i = 0; i < tabKeyCodes.Length; i++)
                    {
                        if (i != index && tabKeyCodes[i] == kcode)
                        {
                            tabKeyCodes[i] = tabKeyCodes[index];
                            texts[i].text = tabKeyCodes[i].ToString();
                        }
                    }

                    tabKeyCodes[index] = kcode;
                    images[index].color = Color.white;
                    texts[index].text = kcode.ToString();
                    keyboard = false;
                    break;
                }
            }
        }
    }

    public void SetIndex(int choisi)
    {
        images[index].color = Color.white;
        index = choisi;
        images[index].color = Color.red;
        btnsImage.gameObject.SetActive(true);
        keyboard = true;
    }

    public void SetResolution(int ResolutionIndex)
    {
        Screen.SetResolution(resolutions[ResolutionIndex].width, resolutions[ResolutionIndex].height, Screen.fullScreen);
        SaveOptions();
    }

    public void SetFullScreen(bool fs)
    {
        Screen.fullScreen = fs;
        SaveOptions();
    }

    public void SetMainVolume(float v)
    {
        audioMixer.SetFloat("MainVolume", Mathf.Log10(v) * 20);
        SaveOptions();
    }

    public void SetMusicVolume(float v)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(v) * 20);
        SaveOptions();
    }

    public void SetEffectVolume(float v)
    {
        audioMixer.SetFloat("EffectVolume", Mathf.Log10(v) * 20);
        SaveOptions();
    }

    void SaveOptions()
    {
        float musicv = 0;
        float mainv = 0;
        float effectv = 0;
        audioMixer.GetFloat("MainVolume", out mainv);
        audioMixer.GetFloat("MusicVolume", out musicv);
        audioMixer.GetFloat("EffectVolume", out effectv);
        SaveScript.SaveOptions(tabKeyCodes, new float[] {mainv, musicv, effectv}, new float[] {smainv.value, smusicv.value, seffectv.value}, resolutionDropdown.value, fsToggle.isOn);
    }
}