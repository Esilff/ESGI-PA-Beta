using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
	public RectTransform mainMenu;
	public RectTransform mapSelectMenu;
	public RectTransform gameConfigMenu;
	public RectTransform optionsMenu;
	public RawImage[] playerImages;
	public RawImage[] mapImages;

	[SerializeField] string map;
	public bool canEditPlayers;
	
	private void Start()
	{
		canEditPlayers = false;
		foreach (var image in playerImages)
		{
			image.color = Color.gray;
		}
		if (Time.timeScale == 0) Time.timeScale = 1;
	}

	public void ToMainMenu()
	{
		canEditPlayers = false;
		mapSelectMenu.gameObject.SetActive(false);
		gameConfigMenu.gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	public void ToOptions()
	{
		mainMenu.gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(true);
	}

	public void ExitOptions()
	{
		optionsMenu.gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	public void ShowMaps()
	{
		mainMenu.gameObject.SetActive(false);
		mapSelectMenu.gameObject.SetActive(true);
	}

	public void PrepareGame()
	{
		canEditPlayers = true;
		mapSelectMenu.gameObject.SetActive(false);
		gameConfigMenu.gameObject.SetActive(true);
	}
	
	public void ChoseMap(string m)
    {
		map = m;
    }

    public void RestartGame()
    {
		Debug.Log("Restart");
		SceneManager.LoadScene(map);
    }

      public void StartGame()
      {
	      foreach (var playerImage in playerImages)
	      {
		      if (playerImage.color == Color.white || playerImage.color == new Color(0.8f, 0.8f, 0.8f, 1.0f))
		      {
			    RestartGame();
		      }
	      }
      }

    public void QuitGame()
    {
		Debug.Log("Quit");
        Application.Quit();
		//quit the game in the editor
	    #if UNITY_EDITOR	
		        UnityEditor.EditorApplication.isPlaying = false;
	    #endif
    }
}
