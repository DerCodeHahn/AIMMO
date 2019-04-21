using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tile : ScriptableObject
{
    #pragma warning disable 0649
    [SerializeField] string tileName;
    [SerializeField] Material material;
    #pragma warning restore 0649
    public Material Material{get{return material;}}

    public string TileName { get => tileName; }

    public abstract void ActionOnMMOAgent(MMOAgent agent);
}
