using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CharacterAI : Agent
{
    public Transform character;
    public Checkpoint[] checkpoints;
    public int currentCheckpoint = 0;

    public Vector2 axis;

    public override void CollectObservations(VectorSensor sensor)
    {
        var position = character.position;
        sensor.AddObservation(position);
        sensor.AddObservation(checkpoints[currentCheckpoint].transform.position);
        
    }
    
    public override void OnEpisodeBegin()
    {
        character.localPosition = new Vector3(-7.6260376f, 1.20520067f, -3.99339294f);
        currentCheckpoint = 0;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float x, y;
        x = actions.DiscreteActions[0] switch
        {
            0 => 0,
            1 => -1,
            2 => 1,
            _ => 0
        };
        y = actions.DiscreteActions[1] switch
        {
            0 => 0,
            1 => -1,
            2 => 1,
            _ => 0
        };
        /*x = actions.ContinuousActions[0];
        y = actions.ContinuousActions[1];*/
        axis = new Vector2(x, y);
        AddReward(-0.05f);
    }

   
}
