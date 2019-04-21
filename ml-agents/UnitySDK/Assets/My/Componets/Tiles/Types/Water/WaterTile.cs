using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterTile", menuName = "AIMMO/Tiles/WaterTile", order = 0)]

public class WaterTile : Tile
{
    [SerializeField] float GiveWater = 1;
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        agent.AddReward(1 - agent.WaterLevel);
        agent.WaterLevel += GiveWater;
        if (agent.FoodLevel > 1)
            agent.FoodLevel = 1;
    }
}
