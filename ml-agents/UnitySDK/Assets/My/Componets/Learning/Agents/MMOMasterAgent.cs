using System;
using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class MMOMasterAgent : Agent
{
    static float maxStock = 1000;
    [SerializeField] MMOTileSelector tileSelector = null;
    LevelGenerator level;
    [SerializeField] Building[] PossibleBuildings;

    Building currentTarget;

    [SerializeField] Resource resourceStock;

    public Resource ResourceStock { get => resourceStock; set => resourceStock = value; }

    public override void AgentReset()
    {
        currentTarget = null;
    }

    internal float[] Observe()
    {
        return resourceStock.Observe();
    }

    public override void CollectObservations()
    {
        if (currentTarget != null)
        {
            if (currentTarget.resourcesCost <= ResourceStock)
            {
                BuildTarget();
            }
        }
        AddVectorObs(Observe());
    }

    private void BuildTarget()
    {
        AddReward(currentTarget.Reward);
        Debug.Log("Build " + currentTarget.name);
        ResourceStock -= currentTarget.resourcesCost;
    }

    private void SetMask()
    {

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //Request Resource
        if (currentTarget != null)
        {
            tileSelector.RequestDecision();
        }
        else//Select Building
        {
            currentTarget = PossibleBuildings[(int)vectorAction[0]]; // 0 = Building Branch
        }
    }

    float Remap01(float inFloat)
    {
        return (inFloat + 1) / 2;
    }

    void ClampToLast(ref int value, int last)
    {
        if (value == last)
            value--;
    }

    private void OnGUI()
    {
        for (int i = 0; i < Resource.resourceCount; i++)
        {
            GUILayout.Label("Resource " + i + ": " + resourceStock[i]);
        }

    }
}
