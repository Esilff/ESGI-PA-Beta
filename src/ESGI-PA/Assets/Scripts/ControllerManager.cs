///using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    // Start is called before the first frame update
    private delegate void OnGamepadUpdate();

    private OnGamepadUpdate _onGamepadUpdate;
    
    private int gamepadCount;
    public static Gamepad[] gamepads;
    void Start()
    {
        _onGamepadUpdate = UpdateGamepads;
        _onGamepadUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamepadCount != Gamepad.all.Count)
        {
            _onGamepadUpdate();
        }
        Debug.Log("Gamepad count : " + gamepadCount);
        Debug.Log("Gamepads : " + gamepads.ToString());
    }

    void UpdateGamepads()
    {
        gamepadCount = Gamepad.all.Count;
        gamepads = Gamepad.all.ToArray();
    }
}
