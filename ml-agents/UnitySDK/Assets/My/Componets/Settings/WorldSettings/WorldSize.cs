using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSize", menuName = "AIMMO/Settings/WorldSize", order = 0)]
public class WorldSize : ScriptableObject
{
    public int SizeX, SizeY;
    public float TileSize;
}

