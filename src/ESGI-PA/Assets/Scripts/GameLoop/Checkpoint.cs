using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameLoop loop;
    public GameLoop Loop
    {
        get => loop;
        set => loop = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        int checkpointIndex = Loop.Checkpoints.IndexOf(gameObject.GetComponent<Checkpoint>());
        var info = Loop.PlayerInfo[other.gameObject];
        info.lastCheckpoint = info.currentCheckpoint;
        info.currentCheckpoint = checkpointIndex;
        if ((checkpointIndex == 0 && info.lastCheckpoint > Loop.Checkpoints.Count * 0.7f) )
        {
            info.turnCount++;
        }
        Debug.Log("Info : " + checkpointIndex + ":" + info.lastCheckpoint + ":" + info.turnCount);
        Loop.PlayerInfo[other.gameObject] = info;
    }
}
