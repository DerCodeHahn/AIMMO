using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
   [SerializeField] Tile tileType;
   public int x,y;

    public Tile TileType { get => tileType; set => tileType = value; }
}
