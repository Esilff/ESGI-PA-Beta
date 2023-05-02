using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject menu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        Debug.Log("Pausing");
        menu.SetActive(!menu.activeSelf);
        Time.timeScale = menu.activeSelf ? 0 : 1;
    }

    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menuprincipal");
    }
}
