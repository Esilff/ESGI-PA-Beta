using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "RaceConfig", menuName = "ScriptableObjects/RaceConfig")]
public class RaceConfig : ScriptableObject
{
    public List<InputDevice> devices;
}
