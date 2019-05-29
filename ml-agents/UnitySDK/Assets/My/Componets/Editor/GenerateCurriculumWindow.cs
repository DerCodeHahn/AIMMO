using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using System;

[System.Serializable]
public class GenerateCurriculumWindow : EditorWindow
{
    readonly string foodRateName = "FoodSpawnRate";
    readonly string waterRateName = "WaterSpawnRate";
    public CurriculumData data;
    public float min_lesson_length;
    public int levelCount;
    public AnimationCurve nextLevelThreshold = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve foodSpawnRate = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve waterSpawnRate = AnimationCurve.Linear(0, 0, 10, 10);

    public string[] variables;

    public string XmlPath;

    [MenuItem("MyTools/GenerateCurriculumWindow")]
    private static void ShowWindow()
    {
        var window = GetWindow<GenerateCurriculumWindow>();
        window.titleContent = new GUIContent("GenerateCurriculumWindow");
        window.Show();
    }

    private void OnGUI()
    {
        EditorStyles.label.wordWrap = true;
        if (data == null)
            data = new CurriculumData();

        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();

        data.min_lesson_length = EditorGUILayout.DelayedIntField("Min Lesson Length", data.min_lesson_length);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Signal Smoothing");
        data.signal_smoothing = EditorGUILayout.Toggle(data.signal_smoothing);
        GUILayout.EndHorizontal();

        levelCount = EditorGUILayout.DelayedIntField("Level Count", levelCount);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Measure Progress Type");
        data.SetMeasure((MeasureType)EditorGUILayout.EnumPopup(data.GetMeasure()));
        GUILayout.EndHorizontal();

        nextLevelThreshold = EditorGUILayout.CurveField("Next Level Threshold", nextLevelThreshold);
        foodSpawnRate = EditorGUILayout.CurveField(foodRateName, foodSpawnRate);
        waterSpawnRate = EditorGUILayout.CurveField(waterRateName, waterSpawnRate);

        EditorGUILayout.LabelField("Path : " + XmlPath);
        if (GUILayout.Button("SetPath"))
        {
            XmlPath = EditorUtility.OpenFilePanel("Select the XML to Override", "", "json");
        }

        if (GUILayout.Button("Load"))
        {
            Load();
        }

        if (GUILayout.Button("Generate"))
        {
            Save();
        }


    }

    private void Load()
    {
        string filedata = File.ReadAllText(XmlPath);
        data = JsonConvert.DeserializeObject<CurriculumData>(filedata);

        levelCount = data.thresholds.Length;

        // Reset old Graphs
        foodSpawnRate.keys = new Keyframe[0];
        nextLevelThreshold.keys = new Keyframe[0];
        waterSpawnRate.keys = new Keyframe[0];

        //Fill with new Data
        for (int i = 0; i < levelCount; i++)
        {
            float amount = i / (levelCount - 1f);
            nextLevelThreshold.AddKey(amount, data.thresholds[i]);
            foodSpawnRate.AddKey(amount, data.parameters[foodRateName][i]);
            waterSpawnRate.AddKey(amount, data.parameters[waterRateName][i]);
        }
    }

    void Save()
    {
        data.thresholds = new float[levelCount];
        data.parameters.Remove(foodRateName); // Empty data
        data.parameters.Remove(waterRateName);

        data.parameters.Add(foodRateName, new float[levelCount]);
        data.parameters.Add(waterRateName, new float[levelCount]);

        for (int i = 0; i < levelCount; i++)
        {
            float amount = (float)i / (float)(levelCount - 1);
            data.thresholds[i] = nextLevelThreshold.Evaluate(amount);
            data.parameters[foodRateName][i] = foodSpawnRate.Evaluate(amount);
            data.parameters[waterRateName][i] = waterSpawnRate.Evaluate(amount);
        }
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        StreamWriter fileStream = File.CreateText(XmlPath);
        fileStream.Write(jsonData);
        fileStream.Flush();
        fileStream.Close();
    }
}

[System.Serializable]
public class CurriculumData
{
    public int min_lesson_length;
    public bool signal_smoothing;
    public float[] thresholds;
    public string measure;
    public Dictionary<string, float[]> parameters;

    public CurriculumData(MeasureType type, int min_lesson_length = 100, bool signal_smoothing = true)
    {
        this.min_lesson_length = min_lesson_length;
        this.signal_smoothing = signal_smoothing;
        parameters = new Dictionary<string, float[]>();
        SetMeasure(type);
    }

    public CurriculumData()
    {
        parameters = new Dictionary<string, float[]>();
    }

    public void SetMeasure(MeasureType type)
    {
        measure = type == MeasureType.progress ? "progress" : "reward";
    }

    public MeasureType GetMeasure()
    {
        return measure == "progress" ? MeasureType.progress : MeasureType.reward;
    }


}

public enum MeasureType
{
    progress,
    reward
}
