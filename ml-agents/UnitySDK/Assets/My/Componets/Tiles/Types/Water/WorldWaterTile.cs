using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWaterTile : WorldTile
{
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        float difference = 1 - agent.WaterLevel;
        agent.AddReward(difference);
        agent.WaterLevel += difference;
    }
    public override Resource GetResource()
    {
        Resource r = new Resource();
        r.Wood = 1;
        return r;
    }
}
