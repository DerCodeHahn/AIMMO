using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLavalTile : WorldTile
{
    public override void ActionOnMMOAgent(MMOAgent agent)
    {
        agent.Die();
        string step = MMOAcademy.instance.GetStepCount() + " ";
        Debug.Log(step + "Lava Death");
    }
}
