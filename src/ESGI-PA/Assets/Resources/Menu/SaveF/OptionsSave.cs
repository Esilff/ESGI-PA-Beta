using UnityEngine;

[System.Serializable]
public class OptionsSave
{
    public KeyCode[] tabKeyCodes {get; private set;}
    public float[] volumes {get; private set;}
    public float[] slidersVolumes {get; private set;}
    public int indexRes {get; private set;}
    public bool isFullScreen {get; private set;}

    public OptionsSave(KeyCode[] keyCodes, float[] vs, float[] sv, int i, bool fs)
    {
        tabKeyCodes = keyCodes;
        volumes = vs;
        slidersVolumes = sv;
        indexRes = i;
        isFullScreen = fs;
    }
}
