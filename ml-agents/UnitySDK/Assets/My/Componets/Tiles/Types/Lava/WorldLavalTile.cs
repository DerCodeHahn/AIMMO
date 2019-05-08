using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLavalTile : WorldTile
{
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        agent.Done();
        agent.SetReward(-1);
        Debug.Log("Tot durch Lava");
    }
}
