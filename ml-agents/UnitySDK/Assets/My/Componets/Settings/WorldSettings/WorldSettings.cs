using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "WorldSettings", menuName = "AIMMO/Settings/WorldSettings", order = 0)]
public class WorldSettings : ScriptableObject
{
    public int SizeX, SizeY;
    public GameObject emptyTile;
    public PrefabSpawnRate[] PrefabSpawnRates;
}
[System.Serializable]
public struct PrefabSpawnRate
{
    public string name;
    public GameObject Prefab;
    public int staticAmount;
    public AnimationCurve curriculumSpawnRates;
    int oldAmount;

    public int OldAmount { get => oldAmount; }
    public void SetAmount(int newAmount)
    {
        oldAmount = newAmount;
    }


}

#if UNITY_EDITOR

[CustomEditor(typeof(WorldSettings))]
public class WorldSettingsEditor : Editor
{
    public AnimationCurve sumCurve;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WorldSettings setting = (WorldSettings)target;
        int sum = 0;
        GUILayout.Label("Number of Tiles: " + (setting.SizeX * setting.SizeY));
        foreach (PrefabSpawnRate prefSpawrate in setting.PrefabSpawnRates)
        {
            sum += prefSpawrate.staticAmount;
        }

        GUILayout.Label("Sum of StaticSpawnRates: " + sum + "/100");

        if (GUILayout.Button("Update Summed Graph"))
        {
            GenerateSumGraph(setting);
        }

        sumCurve = EditorGUILayout.CurveField("Sum of all SpawnRates", sumCurve, null);
    }

    void GenerateSumGraph(WorldSettings setting)
    {
        float steps = 25;
        sumCurve = new AnimationCurve();

        for (int i = 0; i < steps; i++)
        {
            float sum = 0;
            foreach (PrefabSpawnRate prefSpawrate in setting.PrefabSpawnRates)
                sum += prefSpawrate.curriculumSpawnRates.Evaluate(i / steps);
            sumCurve.AddKey(i/ steps, sum);
        }

    }
}
#endif
