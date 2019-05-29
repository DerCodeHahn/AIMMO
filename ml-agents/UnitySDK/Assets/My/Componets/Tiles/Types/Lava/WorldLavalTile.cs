using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLavalTile : WorldTile
{
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        agent.Die();
        Debug.Log("Tot durch Lava");
    }
}
