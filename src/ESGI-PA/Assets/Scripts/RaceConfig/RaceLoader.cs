using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RaceLoader : MonoBehaviour
{
    [SerializeField] private RaceConfig config;

    [SerializeField] private PlayerInputManager manager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UIManager uiManager;
    
    private int playerJoined;

    private Rect[] viewportDuo =
    {
        new Rect(0,0,1,0.5f),
        new Rect(0,0.5f,1,0.5f)
    };
    
    void Start()
    {
        playerJoined = 0;
        manager.playerPrefab = playerPrefab;
        if (config.devices.Count <= 1) manager.splitScreen = false;
        for (var i = 0; i < config.devices.Count; i++)
        {
            
            if (config.devices[i] is Gamepad)
            {
                manager.JoinPlayer(i, i, "gamepad", config.devices[i]);
                continue;
            }
            if (config.devices[i] is Keyboard)
            {
                manager.JoinPlayer(i, i, "keyboard", config.devices[i]);
            }

        }
    }

    public void setViewport(PlayerInput player)
    {
        uiManager.Players.Append(player.gameObject);
        if (config.devices.Count == 1)
        {
            player.gameObject.GetComponentInChildren<Camera>().rect = new Rect(0,0,1,1);
        }
        if (config.devices.Count == 2)
        {
            player.gameObject.GetComponentInChildren<Camera>().rect = viewportDuo[playerJoined];
        }

        playerJoined++;
    }
}
