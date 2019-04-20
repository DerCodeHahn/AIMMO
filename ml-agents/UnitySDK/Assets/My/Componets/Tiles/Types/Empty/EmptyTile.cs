using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyTile", menuName = "AIMMO/Tiles/EmptyTile", order = 0)]
public class EmptyTile : Tile
{
    [SerializeField] float UseWater = 0.1f, UseFood = 0.1f;
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        agent.FoodLevel -= UseFood;
        agent.WaterLevel -= UseWater;
    }
}
