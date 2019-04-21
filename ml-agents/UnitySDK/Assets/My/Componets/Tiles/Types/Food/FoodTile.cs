using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodTile", menuName = "AIMMO/Tiles/FoodTile", order = 0)]
public class FoodTile : Tile
{
    [SerializeField] float GiveFood = 1;
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        agent.AddReward(1 - agent.FoodLevel);
        agent.FoodLevel += GiveFood;
        if (agent.FoodLevel > 1)
            agent.FoodLevel = 1;
    }
}
