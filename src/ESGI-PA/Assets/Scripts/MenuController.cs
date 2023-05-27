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
	public Canvas audio = null;
	public Canvas video = null;
	public Canvas controls = null;
	public UnityEngine.UI.Toggle toggle;
	public Dropdown dropdown;
	private Resolution[] resolutions;
	public UnityEngine.UI.Slider volumeSlider; // Référence au composant Slider qui contrôle le volume
	public AudioSource audioSource; // Référence à votre source audio
	

	[SerializeField] string map;
	public bool canEditPlayers;
	
	private void Start()
	{
		volumeSlider.onValueChanged.AddListener(ChangeVolume);
		resolutions = Screen.resolutions;

		dropdown.ClearOptions();

		// Ajoutez les options de résolution au dropdown
		foreach (Resolution resolution in resolutions)
		{
			dropdown.options.Add(new Dropdown.OptionData(resolution.width + "x" + resolution.height));
		}

		dropdown.value = GetCurrentResolutionIndex();
		dropdown.RefreshShownValue();

		dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
				
		toggle.isOn = Screen.fullScreen;
		toggle.onValueChanged.AddListener(OnToggleValueChanged);
		
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

      public void OptionAudio()
      {
	      Debug.Log("Option Audio");
	      audio.gameObject.SetActive(true);
	      video.gameObject.SetActive(false);
	      controls.gameObject.SetActive(false);

      }
      public void OptionVideo()
	  {
	      Debug.Log("Option Video");
	      audio.gameObject.SetActive(false);
	      video.gameObject.SetActive(true);
	      controls.gameObject.SetActive(false);
	  }
      public void OptionControls()
      	  {
	      Debug.Log("Option Controls");
	      audio.gameObject.SetActive(false);
	      video.gameObject.SetActive(false);
	      controls.gameObject.SetActive(true);
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
      private void OnToggleValueChanged(bool isFullscreen)
      {
	      Screen.fullScreen = isFullscreen;
      }
      private int GetCurrentResolutionIndex()
      {
	      Resolution currentResolution = Screen.currentResolution;

	      for (int i = 0; i < resolutions.Length; i++)
	      {
		      if (resolutions[i].width == currentResolution.width &&
		          resolutions[i].height == currentResolution.height)
		      {
			      return i;
		      }
	      }

	      return 0;
      }

      private void OnDropdownValueChanged(int index)
      {
	      Resolution resolution = resolutions[index];
	      Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
      }
      private void ChangeVolume(float volume)
      {
	      audioSource.volume = volume;
      }
}
