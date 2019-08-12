using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStoneTile : WorldTile
{
    public override Resource GetResource()
    {
        Resource r = new Resource();
        r.Stone = 1;
        return r;
    }
}
