using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Resource
{
    static int maxStack = 1000;
    public static int resourceCount = 3;
    [SerializeField] float food;
    [SerializeField] float wood;
    [SerializeField] float stone;

    public float Food { get => food; set => food = value; }
    public float Wood { get => wood; set => wood = value; }
    public float Stone { get => stone; set => stone = value; }

    public float[] Observe()
    {
        return new float[] { food / maxStack, wood / maxStack, stone / maxStack };
    }

    public static Resource operator +(Resource a, Resource b)
    {
        for (int i = 0; i < resourceCount; i++)
        {
            a[i] += b[i];
        }
        return a;
    }
    public static Resource operator -(Resource a, Resource b)
    {
        for (int i = 0; i < resourceCount; i++)
        {
            a[i] -= b[i];
        }
        return a;
    }
    //Kosten <= Lager
    public static bool operator <=(Resource r1, Resource r2)
    {
        for (int i = 0; i < resourceCount; i++)
            if (r1[i] > r2[i])
                return false;
        return true;
    }
    public static bool operator >=(Resource r1, Resource r2)
    {
        for (int i = 0; i < resourceCount; i++)
            if (r1[i] < r2[i])
                return false;
        return true;
    }


    public float this[int key]
    {
        get
        {
            switch (key)
            {
                case 0:
                    return food;
                case 1:
                    return wood;
                case 2:
                    return stone;
                default:
                    throw new System.IndexOutOfRangeException();
            }
        }
        set
        {
            switch (key)
            {
                case 0:
                    food = value;
                    break;
                case 1:
                    wood = value;
                    break;
                case 2:
                    stone = value;
                    break;
                default:
                    throw new System.IndexOutOfRangeException();
            }
        }
    }
}
