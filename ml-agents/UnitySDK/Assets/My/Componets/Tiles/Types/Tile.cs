using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tile : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField] string tileName;
#pragma warning restore 0649
    public string TileName { get => tileName; }
}
