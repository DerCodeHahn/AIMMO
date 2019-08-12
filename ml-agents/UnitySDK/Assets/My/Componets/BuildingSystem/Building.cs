using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ResourceSystem/Building", order = 0)]
public class Building : ScriptableObject
{
    public Resource resourcesCost;
    public float Reward = 1;

    public float[] Observe()
    {
        return resourcesCost.Observe();
    }

}
