using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RaceMapper : EditorWindow
{
    [MenuItem("Window/RaceMapper")]
    public static void ShowWindow()
    {
        GetWindow<RaceMapper>();
    }
    private void OnGUI()
    {
        GUILayout.Label("Options", EditorStyles.boldLabel);
    }
}
