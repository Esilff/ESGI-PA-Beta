using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] menus;

    private void Start()
    {
        Time.timeScale = 1;
        var o = SaveScript.LoadOptionsSaves();
        var om = GameObject.Find("OptionsMenu").GetComponent<OptionMenu>();
        om.SetMainVolume(o.slidersVolumes[0]);
        om.SetMusicVolume(o.slidersVolumes[1]);
        om.SetEffectVolume(o.slidersVolumes[2]);
        for (int i = 0; i < menus.Length; i++)
            menus[i].SetActive(false);

        menus[0].SetActive(true);
    }

    public void ChangeMenu(int menuIndex)
    {
        for (int i = 0; i < menus.Length; i++)
            menus[i].SetActive(i == menuIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}