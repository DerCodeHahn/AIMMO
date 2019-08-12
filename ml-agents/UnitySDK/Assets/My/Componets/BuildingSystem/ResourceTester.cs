using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTester : MonoBehaviour
{
    Resource r1 = new Resource();
    Resource r2 = new Resource();
    // Start is called before the first frame update
    void Start()
    {

        r1.Food = 10;
        r1.Stone = 2;
        r2.Food = 5;

        DLog();
    }

    void DLog()
    {
        Debug.Log(r1 <= r2);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("+1"))
        {
            r2.Food++;
            DLog();
        }
        if (GUILayout.Button("+1"))
        {
            r2.Stone++;
            DLog();
        }
        if(GUILayout.Button("Build"))
        {
            r2 -= r1;
        }
    }
}
