using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum RaceObject
{
    CHECKPOINT, BOX, RESPAWN
}

public class RaceMapper : EditorWindow
{
    private bool _canPlaceItem;
    private RaceObject objectToPlace;
    
    [SerializeField] private GameObject
    [MenuItem("Window/RaceMapper")]
    public static void ShowWindow()
    {
        GetWindow<RaceMapper>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += SetObject;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SetObject;
    }

    private void OnGUI()
    {
        GUILayout.Label("Options", EditorStyles.boldLabel);
        if (GUILayout.Button("Checkpoint"))
        {
            _canPlaceItem = true;
        }
    }

    private void SetObject(SceneView sceneView)
    {
        if (!_canPlaceItem) return;
        var currentEvent = Event.current;
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {
            Vector2 mousePosition = currentEvent.mousePosition;
            mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y;
            Ray ray = sceneView.camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPos = hit.point;
                hitPos.y += 2;
                switch (expression)
                {
                    
                }
            }
        }
    }
}
