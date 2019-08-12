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
    [SerializeField] bool UseCurriculumWorld;
    GameObject[,] world;
    Transform worldTransfrom;

    public GameObject GetTile(int x, int y)
    {
        return world[x, y];
    }

    [SerializeField] SavedSeed seed;
#pragma warning restore 0649

    public SavedSeed Seed { get => seed; set => seed = value; }
    public WorldSettings WorldSetting { get => worldSetting; }
    List<GameObject> WorldBluePrint;
    float waterSpawnRate;
    float foodSpawnRate;
    void Start()
    {
        if (seed != null)
            UnityEngine.Random.state = seed.state;
        Init();
        UpdateWorld();
    }

    void Init()
    {
        worldTransfrom = new GameObject().transform;
        worldTransfrom.name = "World";
        world = new GameObject[worldSetting.SizeX, worldSetting.SizeY];


        for (int y = 0; y < worldSetting.SizeY; y++)
        {
            Transform parentTransform = new GameObject().transform;
            parentTransform.SetParent(worldTransfrom);
            parentTransform.name = "row" + y;
        }
        // if (MMOAcademy.instance.GetIsInference())
            GenerateBluePrint();
        Regenerate();
    }

    //TODO: Mögliche Speicher Optimierung nur das Löschen was zu viel und auch nur das hinzufügen was gebraucht wird
    void GenerateBluePrint()
    {
        WorldBluePrint = new List<GameObject>();

        foreach (PrefabSpawnRate prefabSpawnRate in worldSetting.PrefabSpawnRates)
        {
            int amount = 0;
            if (UseCurriculumWorld && !MMOAcademy.instance.GetIsInference())
            {
                if (MMOAcademy.instance.resetParameters.ContainsKey(prefabSpawnRate.name))
                {
                    int currentSpawnRate = (int)MMOAcademy.instance.resetParameters[prefabSpawnRate.name];
                    amount = (int)((currentSpawnRate / 100f) * worldSetting.SizeX * worldSetting.SizeY);
                }
                else// Use Default if not in Use
                    amount = (int)((prefabSpawnRate.staticAmount / 100f) * worldSetting.SizeX * worldSetting.SizeY);

            }
            else
                amount = (int)((prefabSpawnRate.staticAmount / 100f) * worldSetting.SizeX * worldSetting.SizeY);
            for (int i = 0; i < amount; i++)
                WorldBluePrint.Add(Instantiate(prefabSpawnRate.Prefab));

        }
        while (WorldBluePrint.Count < worldSetting.SizeX * worldSetting.SizeY) //fill With empty
        {
            WorldBluePrint.Add(Instantiate(worldSetting.emptyTile));
        }

    }
    void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    void UpdateWorld()
    {
        IEnumerator<GameObject> blueprintEnumerable = WorldBluePrint.GetEnumerator();
        blueprintEnumerable.MoveNext();
        for (int y = 0; y < worldSetting.SizeY; y++)
        {
            for (int x = 0; x < worldSetting.SizeX; x++)
            {
                GameObject newTile = blueprintEnumerable.Current;
                blueprintEnumerable.MoveNext();
                WorldTile worldTile = newTile.GetComponent<WorldTile>();
                newTile.transform.SetParent(worldTransfrom.GetChild(y));
                worldTile.x = x;
                worldTile.y = y;
                worldTile.ResetTile();
                newTile.transform.localScale = Vector3.one;
                newTile.transform.position = new Vector3((x - worldSetting.SizeX / 2),
                                                         (y - worldSetting.SizeY / 2), 0);
                world[x, y] = newTile;
            }
        }

    }

    internal void Regenerate()
    {
        if (!MMOAcademy.instance.GetIsInference() && UseCurriculumWorld)
        {
            bool regenerateBluePrint = false;

            foreach (PrefabSpawnRate spawnRate in WorldSetting.PrefabSpawnRates)
            {
                float newAmount = MMOAcademy.instance.resetParameters[spawnRate.name];
                if (newAmount != spawnRate.OldAmount)
                {
                    spawnRate.SetAmount((int)newAmount);
                    regenerateBluePrint = true;
                }

            }
            if (regenerateBluePrint)
                GenerateBluePrint();
        }
        Shuffle(WorldBluePrint);
        UpdateWorld();
    }

    public GameObject GetTileFromWorldPos(Vector3 pos)
    {
        int x = Mathf.RoundToInt((pos.x + worldSetting.SizeX / 2));
        int y = Mathf.RoundToInt((pos.y + worldSetting.SizeY / 2));
        return world[x, y];
    }
    [System.Serializable]
    public struct ResourceLink
    {
        public string NameInCurriculum;
        float value;

        public float Value { get => value; set => this.value = value; }
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


