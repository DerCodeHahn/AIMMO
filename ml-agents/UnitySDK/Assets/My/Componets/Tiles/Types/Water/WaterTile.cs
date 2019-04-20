using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterTile", menuName = "AIMMO/Tiles/WaterTile", order = 0)]

public class WaterTile : Tile
{
    [SerializeField] float GiveWater = 1;
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        agent.WaterLevel += GiveWater;
    }
}
