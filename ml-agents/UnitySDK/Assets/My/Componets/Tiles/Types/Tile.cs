using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tile : ScriptableObject
{
    [SerializeField] string tileName;
    [SerializeField] Material material;

    public Material Material{get{return material;}}

    public string TileName { get => tileName; }

    public abstract void ActionOnMMOAgent(MMOAgent agent);
}
