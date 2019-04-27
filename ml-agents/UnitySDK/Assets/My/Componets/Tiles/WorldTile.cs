using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
    [SerializeField] protected Color tileColor;
    public int x, y;

    new MeshRenderer renderer;
    MaterialPropertyBlock props;

    private void Awake()
    {
        props = new MaterialPropertyBlock();
        renderer = GetComponent<MeshRenderer>();
        SetMaterialPropertyColor(tileColor);
    }

    protected void SetMaterialPropertyColor(Color c)
    {
        props.SetColor("_Color", c);
        renderer.SetPropertyBlock(props);
    }
    public virtual void ActionOnMMOAgent(MMOAgent agent)
    {

    }
}
