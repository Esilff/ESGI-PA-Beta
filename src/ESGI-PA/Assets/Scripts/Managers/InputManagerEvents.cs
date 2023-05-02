using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = System.Object;


public class InputManagerEvents : MonoBehaviour
{
    private const int MAX_DEVICES = 4;

    [SerializeField] private RaceConfig raceConfig;

    [SerializeField] private MenuController menu;
    
    private bool isKeyboardActive;

    private List<InputDevice> activeDevices = new();
    private Keyboard keyboard;

    void Start()
    {
        keyboard = InputSystem.GetDevice<Keyboard>();
    }
    private void Update()
    {
        if (menu.canEditPlayers)
        {
            detectDevices();
            updateUI();
        }
    }

    private void detectDevices()
    {
        detectKeyboard();
        detectGamepads();
    }

    private void detectKeyboard()
    {
        if (keyboard.anyKey.isPressed && !keyboard.backspaceKey.isPressed && !isKeyboardActive && activeDevices.Count < MAX_DEVICES)
        {
            isKeyboardActive = true;
            activeDevices.Add(keyboard);
        }
        else if (keyboard.backspaceKey.isPressed && isKeyboardActive)
        {
            isKeyboardActive = false;
            activeDevices.Remove(keyboard);
        }
    }

    private void detectGamepads()
    {
        //On regarde tous les gamepads dispo, si parmi l'un de ces gamepads, l'une des touches est jouée, on active le gamepad
        Gamepad[] gamepads = Gamepad.all.ToArray();
        for (var i = 0; i < gamepads.Length; i++)
        {
            if (gamepads[i].buttonSouth.isPressed && !activeDevices.Contains(gamepads[i]) && activeDevices.Count < MAX_DEVICES)
            {
                activeDevices.Add(gamepads[i]);
            }
        }
        foreach (var activeDevice in activeDevices)
        {
            if (activeDevice is Gamepad)
            {
                Gamepad device = (Gamepad)activeDevice;
                if (!Gamepad.all.ToArray().Contains(device) || device.buttonEast.isPressed)
                {
                    activeDevices.Remove(device);
                    break;
                }
            }
        }
    }

    private void updateUI()
    {
        for (var i = 0; i < 4; i++)
        {
            if (i < activeDevices.Count)
            {
                if (activeDevices[i] is Keyboard)
                {
                    menu.playerImages[i].color = new Color(0.8f,0.8f,0.8f,1.0f);
                }
                else
                {
                    menu.playerImages[i].color = Color.white;
                }
                
            }
            else
            {
                menu.playerImages[i].color = Color.grey;
            }
        }
    }

    private void OnDestroy()
    {
        raceConfig.devices = activeDevices;
    }
}