using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDependCameraSize : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField] LevelGenerator LevelGenerator;

    #pragma warning restore 0649
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().orthographicSize = LevelGenerator.WorldSetting.SizeY / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
