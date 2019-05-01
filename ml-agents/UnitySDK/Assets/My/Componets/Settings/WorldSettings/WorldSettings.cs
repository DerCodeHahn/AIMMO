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
    public PrefabSpawnRate[] PrefabSpawnRates;
}
[System.Serializable]
public struct PrefabSpawnRate
{
    public string name;
    public GameObject Prefab;
    public int Amount;
}

#if UNITY_EDITOR

[CustomEditor(typeof(WorldSettings))]
public class WorldSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WorldSettings setting = (WorldSettings)target;
        int sum = 0;
        GUILayout.Label("Number of Tiles: " + (setting.SizeX * setting.SizeY));
        foreach (PrefabSpawnRate prefSpawrate in setting.PrefabSpawnRates)
        {
            sum += prefSpawrate.Amount;
        }
        if (sum == 100)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        GUILayout.Label("Sum of SpawnRates: " + sum + "/100");
    }
}
#endif
