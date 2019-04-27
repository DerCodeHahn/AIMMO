using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class LevelGenerator : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] WorldSettings worldSetting;
    GameObject[,] world;
    Transform worldTransfrom;

    [SerializeField] SavedSeed seed;
#pragma warning restore 0649

    public SavedSeed Seed { get => seed; set => seed = value; }
    public WorldSettings WorldSetting { get => worldSetting; }

    void Start()
    {
        if (seed != null)
            UnityEngine.Random.state = seed.state;
        Generate();
    }

    void Generate()
    {
        worldTransfrom = new GameObject().transform;
        worldTransfrom.name = "World";
        world = new GameObject[worldSetting.SizeX, worldSetting.SizeY];
        
        for (int y = 0; y < worldSetting.SizeY; y++)
        {
            Transform parentTransform = new GameObject().transform;
            parentTransform.SetParent(worldTransfrom);
            parentTransform.name = "row" + y;

            for (int x = 0; x < worldSetting.SizeX; x++)
            {
                GameObject newTile = Instantiate(RandomTile(), parentTransform);
                
                WorldTile worldTile = newTile.GetComponent<WorldTile>();

                worldTile.x = x;
                worldTile.y = y;
                newTile.transform.localScale = Vector3.one;
                newTile.transform.position = new Vector3((x - worldSetting.SizeX / 2),
                                                         (y - worldSetting.SizeY / 2), 0);
                
                world[x, y] = newTile;
            }
        }
    }

    GameObject RandomTile(){
        int comulativeChance = 0;
        int rndNumber = UnityEngine.Random.Range(0, 100);
        for (int i = 0; i < worldSetting.PrefabSpawnRates.Length; i++)
        {
            comulativeChance += worldSetting.PrefabSpawnRates[i].Amount;
            if(rndNumber <= comulativeChance)
                return worldSetting.PrefabSpawnRates[i].Prefab;
        }
        return null;
    }

    internal void Regenerate()
    {
        Destroy(worldTransfrom.gameObject);
        Generate();
    }

    public GameObject GetTileFromWorldPos(Vector3 pos)
    {
        int x = Mathf.RoundToInt((pos.x + worldSetting.SizeX / 2));
        int y = Mathf.RoundToInt((pos.y + worldSetting.SizeY / 2));
        return world[x, y];
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(LevelGenerator))]
public class LevelGerneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelGenerator generator = (LevelGenerator)target;
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Save Seed"))
            {
                if (savePathForSeed.Length != 0)
                {//Random.state;
                    SavedSeed seed = (SavedSeed)ScriptableObject.CreateInstance("SavedSeed");
                    seed.state = UnityEngine.Random.state;
                    AssetDatabase.CreateAsset(seed, savePathForSeed);
                    //TODO : Dialog to get name
                    Debug.Log("Saved Seed");
                }
            }
            //EditorUtility.DisplayDialog("Not possible", "You must enter playmode first!", "OK");

        }
        if (generator.Seed != null && GUILayout.Button("Remove Seed"))
        {
            generator.Seed = null;
        }
    }

    private static bool prefsLoaded = false;

    // The Preferences
    public static string savePathForSeed = "";

    [PreferenceItem("AIMMO Preferences")]
    public static void PreferencesGUI()
    {
        // Load the preferences
        if (!prefsLoaded)
        {
            savePathForSeed = EditorPrefs.GetString("SavePathForSeedKey", "/Assets");
            prefsLoaded = true;
        }

        // Preferences GUI
        EditorGUILayout.LabelField("Save Seed File under: " + savePathForSeed);
        if (GUILayout.Button("Change Save Folder"))
            savePathForSeed = EditorUtility.SaveFilePanelInProject("Save Seed", "Seed", "asset", "Please enter a file name to save the texture to");

        // Save the preferences
        if (GUI.changed)
            EditorPrefs.SetString("SavePathForSeedKey", savePathForSeed);
    }
}
#endif


