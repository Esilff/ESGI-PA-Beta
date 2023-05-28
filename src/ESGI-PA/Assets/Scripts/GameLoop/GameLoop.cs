using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PlayerState
{
    public int turnCount;
    public int currentCheckpoint;
    public int lastCheckpoint;
}

public class GameLoop : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkpoints;
    private GameObject testIndex;
    public List<Checkpoint> Checkpoints
    {
        get => checkpoints;
    }

    private Dictionary<GameObject, PlayerState> playersInfo = new();

    public Dictionary<GameObject, PlayerState> PlayerInfo
    {
        get => playersInfo;
        set => playersInfo = value;
    }

    public int maxTurn = 2;

    private bool hasStarted = false;

    private List<GameObject> players = new();

    public List<GameObject> PlayersRank
    {
        get => players;
        set => players = value;
    }

    void Start()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.Loop = gameObject.GetComponent<GameLoop>();
        }
    }

    void Update()
    {
        if (players == null) return;
        players.Sort(((o, o1) => (PlacementScore(o) > PlacementScore(o1)) ? -1 : 
            (PlacementScore(o) < PlacementScore(o1)) ? 1 : 0));
        if (CheckEndgame())
        {
            // Debug.Log("Game has ended");
            SceneManager.LoadScene("Menuprincipal");
        }
    }

    

    private bool CheckEndgame()
    {
        if (!hasStarted) return false;
        if (playersInfo.Count < 1)
        {
            return false;
        }    
        foreach (var player in players)
        {
            if (playersInfo[player].turnCount < 2)
            {
                return false;
            }
        }
        return true;
    }

    private int PlacementScore(GameObject player)
    {
        int nextCheckpoint = playersInfo[player].currentCheckpoint + 1;
        Transform nextCheckpointPos =
            Checkpoints[(nextCheckpoint > Checkpoints.Count) ? 0 : nextCheckpoint].gameObject.transform;
        int distance = (int)(Mathf.Abs(nextCheckpointPos.position.x - player.transform.position.x) + Mathf.Abs(nextCheckpointPos.position.y - player.transform.position.y));
        return 100 + (playersInfo[player].turnCount * 1000) - distance;
    }
    
    public void AddPlayer(GameObject obj)
    {
        if (!obj.CompareTag("Player")) return;
        PlayersRank.Add(obj);
        PlayerInfo.Add(obj, new PlayerState());
    }
}
