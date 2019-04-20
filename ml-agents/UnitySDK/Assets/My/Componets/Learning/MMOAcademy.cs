using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class MMOAcademy : Academy
{
    public static MMOAcademy instance;
    [Header("MMOAcademy Spezific"), Space(5)]
    [SerializeField] LevelGenerator levelGenerator;
    [SerializeField] bool randomizeEachRun = true;

    public LevelGenerator LevelGenerator { get => levelGenerator; }

    private new void Awake()
    {
        base.Awake();
        if (instance != null)
            Debug.Log("instance already set !", gameObject);
        instance = this;
    }

    public override void AcademyReset()
    {
        if(randomizeEachRun)
        {
            LevelGenerator.Regenerate();
        }
    }

    public override void AcademyStep()
    {

    }
}
