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
    [SerializeField] WorldSize worldSize;
    [SerializeField] Tile[] tiles;
    [SerializeField] GameObject TilePrefab;
    GameObject[,] world;
    Transform worldTransfrom;

    [SerializeField] SavedSeed seed;

    public SavedSeed Seed { get => seed; set => seed = value; }
    public WorldSize WorldSize { get => worldSize; }


    // Start is called before the first frame update
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
        world = new GameObject[worldSize.SizeX, worldSize.SizeY];

        for (int y = 0; y < worldSize.SizeY; y++)
        {
            Transform parentTransform = new GameObject().transform;
            parentTransform.SetParent(worldTransfrom);
            parentTransform.name = "row" + y;

            for (int x = 0; x < worldSize.SizeX; x++)
            {
                GameObject newTile = Instantiate(TilePrefab, parentTransform);
                int rndNumber = UnityEngine.Random.Range(0, tiles.Length);
                newTile.name = tiles[rndNumber].name + x;
                WorldTile worldTile = newTile.GetComponent<WorldTile>();
                worldTile.TileType = tiles[rndNumber];
                worldTile.x = x;
                worldTile.y = y;
                newTile.transform.localScale = Vector3.one * worldSize.TileSize;
                newTile.transform.position = new Vector3((x - worldSize.SizeX / 2) * worldSize.TileSize,
                                                         (y - worldSize.SizeY / 2) * worldSize.TileSize, 0);
                newTile.GetComponent<MeshRenderer>().sharedMaterial = tiles[rndNumber].Material;
                world[x, y] = newTile;
            }
        }
    }

    internal void Regenerate()
    {
        Destroy(worldTransfrom.gameObject);
        Generate();
    }

    public GameObject GetTileFromWorldPos(Vector3 pos)
    {
        int x = Mathf.RoundToInt(((pos.x + worldSize.SizeX / 2) * worldSize.TileSize) / worldSize.TileSize);
        int y = Mathf.RoundToInt(((pos.y + worldSize.SizeY / 2) * worldSize.TileSize) / worldSize.TileSize);
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


