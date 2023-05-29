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

    private void Start()
    {
        this.loop = FindObjectOfType<GameLoop>().GetComponent<GameLoop>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<PhysicsVehicle>(out PhysicsVehicle pv);
        if (pv) pv.lastCheckpoint = this;
        if (!other.CompareTag("Player")) return;
        other.gameObject.GetComponent<PhysicCharacter>().agent.SetDestination(
            loop.Checkpoints[
                loop.Checkpoints.IndexOf(this) == loop.Checkpoints.Count - 1 ?
                    0 : loop.Checkpoints.IndexOf(this) + 1
            ].transform.position
            );
        int checkpointIndex = Loop.Checkpoints.IndexOf(gameObject.GetComponent<Checkpoint>());
        var info = Loop.PlayerInfo[other.gameObject];
        info.lastCheckpoint = info.currentCheckpoint;
        info.currentCheckpoint = checkpointIndex;
        if ((checkpointIndex == 0 && info.lastCheckpoint > Loop.Checkpoints.Count * 0.7f) )
        {
            info.turnCount++;
        }
        Loop.PlayerInfo[other.gameObject] = info;
    }
}
