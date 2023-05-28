using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

enum RaceObject
{
    Checkpoint, Box, Respawn
}

public class RaceMapper : EditorWindow
{
    private bool _canPlaceItem;
    private RaceObject _objectToPlace;

    private GameObject _checkpointObject;
    private RaceLoader _loader;
    private GameObject _boxObject;
    private GameLoop _gameLoop;

    private string _objectLabel = "None";
    private string _heightOffset = "1";
    private string _scale = "1";
    private bool validOffset = true;
    private bool validScale = true;
    [MenuItem("Window/RaceMapper")]
    public static void ShowWindow()
    {
        GetWindow<RaceMapper>();
    }

    private void OnEnable()
    {
        _loader = FindObjectOfType(typeof(RaceLoader)).GetComponent<RaceLoader>();
        _gameLoop = FindObjectOfType(typeof(GameLoop)).GetComponent<GameLoop>();
        _checkpointObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/checkpoint.prefab");
        _boxObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/bonus.prefab");
        SceneView.duringSceneGui += SetObject;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SetObject;
    }

    private void OnGUI()
    {
        GUILayout.Label("Mapper", EditorStyles.boldLabel);
        if (GUILayout.Button("Checkpoint"))
        {
            _canPlaceItem = true;
            _objectToPlace = RaceObject.Checkpoint;
            _objectLabel = "checkpoint";
        }
        if (GUILayout.Button("Respawn"))
        {
            _canPlaceItem = true;
            _objectToPlace = RaceObject.Respawn;
            _objectLabel = "respawn";
        }
        if (GUILayout.Button("Box"))
        {
            _canPlaceItem = true;
            _objectToPlace = RaceObject.Box;
            _objectLabel = "respawn";
        }
        GUILayout.Label("Advanced options", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ground offset");
        _heightOffset = EditorGUILayout.TextArea(_heightOffset, GUILayout.ExpandHeight(false));
        GUILayout.EndHorizontal();
        if (!validOffset) EditorGUILayout.LabelField("<color=yellow>[Warning] : The offset is not a number, default offset will be set to 1</color>", new GUIStyle(GUI.skin.label) {richText = true});
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Scale");
        _scale = EditorGUILayout.TextArea(_scale, GUILayout.ExpandHeight(false));
        GUILayout.EndHorizontal();
        if (!validScale) EditorGUILayout.LabelField("<color=yellow>[Warning] : The scale is not a number, default scale will be set to 1</color>", new GUIStyle(GUI.skin.label) {richText = true});
        GUILayout.Label($"Current object : {_objectLabel}");
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
                validOffset = int.TryParse(_heightOffset, out int num);
                hitPos.y += validOffset ? num : 1;
                validScale = int.TryParse(_scale, out int scaleValue);
                switch (_objectToPlace)
                {
                    case RaceObject.Checkpoint:
                        GameObject checkpoint = Instantiate(_checkpointObject, hitPos, Quaternion.identity);
                        checkpoint.transform.localScale *= validScale ? scaleValue : 1;
                        _gameLoop.Checkpoints.Add(checkpoint.GetComponent<Checkpoint>());
                        break;
                    case RaceObject.Respawn:
                        _loader.spawningPosition.Add(hitPos);
                        break;
                    case RaceObject.Box:
                        GameObject box = Instantiate(_boxObject, hitPos, Quaternion.identity);
                        box.transform.localScale *= validScale ? scaleValue : 1;
                        break;
                }
                _canPlaceItem = false;
            }
        }
    }
}
