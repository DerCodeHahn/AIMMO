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
        difference = Mathf.Clamp(difference, 0, FoodStorage);
        agent.AddReward(difference);
        FoodStorage -= difference;
        agent.FoodLevel += difference;

        if (agent.CurrentWorldTile != null)
        {
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        Color currentColor = tileColor;
        currentColor.g = FoodStorage / FoodStorageMaximunm;
        SetMaterialPropertyColor(currentColor);
    }

    private void FixedUpdate()
    {
        if (FoodStorage < FoodStorageMaximunm)
        {
            FoodStorage += FoodStorageFillRate;
            UpdateColor();
        }
    }
}
