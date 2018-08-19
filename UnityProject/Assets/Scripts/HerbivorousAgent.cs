﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

/// <summary>
/// This class handles the behaviour of the herbivorous agent
/// TODO : possibility that we can regroup the common code of multiple agents into a mother abstract class
/// </summary>
public class HerbivorousAgent : LivingBeingAgent
{

    public Herbivorous getLivingBeing()
    {
        return (Herbivorous)livingBeing;
    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        livingBeing = new Herbivorous(99, 0, 20, 99, 0);
    }

    public override void CollectObservations()
    {
        if (useVectorObs)
        {
            var rayDistance = 5f;
            float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
            var detectableObjects = new[] { "food" };
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
            AddVectorObs(gameObject.transform.rotation.z);
            AddVectorObs(gameObject.transform.rotation.x);
        }

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // AddReward(-0.01f);


        // Reset every 1000 actions or when the agent fell
        if (amountActions > 1000 || transform.position.y < 0)
        {
            amountActions = 0;
            Done(); 
        }
        // Move
        transform.Rotate(new Vector3(0, Mathf.Clamp(vectorAction[1], -1f, 1f), 0), Time.fixedDeltaTime * 500);
        transform.Translate(new Vector3(0, 0, 0.1f) * Mathf.Clamp(vectorAction[0], 0f , 2f));
        
        if (livingBeing.Life == 0) // Dead
        {

            AddReward(-10f);
            Done();
        }
        else
            AddReward(0.01f);
           
        if(amountActions > 10) // After a certain amount of actions
            previousLife = livingBeing.Life;

        amountActions++;
    }
}