using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RaceLoader : MonoBehaviour
{
    [SerializeField] private RaceConfig config;

    [SerializeField] private PlayerInputManager manager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameLoop loop;

    private PlayerInput lastInput;
    
    private int _playerJoined = 0;

    public List<Vector3> spawningPosition = new();

    public RaceConfig Config
    {
        get => config;
        set => config = value;
    }

    private Rect[] viewportDuo =
    {
        new Rect(0,0,1,0.5f),
        new Rect(0,0.5f,1,0.5f)
    };
    
    void Start()
    {
        if (config.players == null) LoadFirstGame();
        else LoadGameFromConfigFile();
    }

    private void LoadGameFromConfigFile()
    {
        foreach (var player in config.players)
        {
            SetPlayer(player.GetComponent<PlayerInput>());
        }
    }

    private void LoadFirstGame()
    {
        _playerJoined = 0;
        manager.playerPrefab = playerPrefab;
        if (config.devices.Count <= 1) manager.splitScreen = false;
        for (var i = 0; i < config.devices.Count; i++)
        {
            manager.JoinPlayer(i, i, config.devices[i] is Gamepad ? "gamepad" : "keyboard", config.devices[i]);
        }
    }

    public void SetPlayer(PlayerInput player)
    {
        
        // uiManager.Players.Append(player.gameObject);
        // loop.PlayersRank.Append(player.gameObject);
        loop.AddPlayer(player.gameObject);
        //uiManager.LinkToUI(player.gameObject);
        SetCameraLayout(player.GetComponent<PhysicCharacter>().camera.GetComponent<Camera>());
        

        player.gameObject.transform.position = spawningPosition[_playerJoined];
        _playerJoined++;
    }

    private void SetCameraLayout(Camera camera)
    {
        if (config.devices.Count == 1)
        {
            camera.rect = new Rect(0,0,1,1);
        }
        if (config.devices.Count == 2)
        {
            camera.rect = viewportDuo[_playerJoined];
        }

        camera.transform.parent = null;
    }
}
