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
    
    private int playerJoined;

    public List<Vector3> spawningPosition = new();

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
            manager.JoinPlayer(i, i, config.devices[i] is Gamepad ? "gamepad" : "keyboard", config.devices[i]);
        }


            GameObject bot = Instantiate(manager.playerPrefab);
            bot.transform.position = spawningPosition[playerJoined];
            PhysicCharacter character =  bot.GetComponent<PhysicCharacter>();
            character.isIAControlled = true;
            character.firstDestination = loop.Checkpoints[0].transform;
            character.agent.enabled = true;
            bot.GetComponent<PlayerInput>().enabled = false;
            loop.AddPlayer(bot);
            playerJoined++;
        }

    public void SetPlayer(PlayerInput player)
    {
        loop.AddPlayer(player.gameObject);
        SetCameraLayout(player.GetComponent<PhysicCharacter>().camera.GetComponent<Camera>());
        

        player.gameObject.transform.position = spawningPosition[playerJoined];
        playerJoined++;
    }

    private void SetCameraLayout(Camera camera)
    {
        if (config.devices.Count == 1)
        {
            camera.rect = new Rect(0,0,1,1);
        }
        if (config.devices.Count == 2)
        {
            camera.rect = viewportDuo[playerJoined];
        }

        camera.transform.parent = null;
    }
}
