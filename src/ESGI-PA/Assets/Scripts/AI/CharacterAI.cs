using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CharacterAI : Agent
{
    public Transform character;
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(actions.DiscreteActions[0]);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(character.position);
    }
}
