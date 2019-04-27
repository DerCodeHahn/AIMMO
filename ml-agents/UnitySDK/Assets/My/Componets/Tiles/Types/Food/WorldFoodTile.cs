using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFoodTile : WorldTile
{
    [SerializeField] float FoodStorage = 1;
    [SerializeField] float FoodStorageMaximunm = 1;
    [SerializeField] float FoodStorageFillRate = 0.01f;

    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        float difference = 1 - agent.FoodLevel;
        agent.AddReward(difference);
        agent.FoodLevel += difference;
        FoodStorage -= difference;

        if (agent.FoodLevel > 1)
            agent.FoodLevel = 1;

        if (agent.CurrentWorldTile != null)
        {
            Color currentColor = tileColor;
            currentColor.g = FoodStorage / FoodStorageMaximunm;
            SetMaterialPropertyColor(currentColor);
        }
    }
    private void Update()
    {
        if (FoodStorage < FoodStorageMaximunm)
        {
            FoodStorage += FoodStorageFillRate;
        }
    }
}
