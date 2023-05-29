using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode
{
    Cup, Free
}

[CreateAssetMenu(fileName = "RaceConfig", menuName = "ScriptableObjects/RaceConfig")]
public class RaceConfig : ScriptableObject
{
    public List<InputDevice> devices;
    public List<GameObject> players;
    public GameMode mode;
}
