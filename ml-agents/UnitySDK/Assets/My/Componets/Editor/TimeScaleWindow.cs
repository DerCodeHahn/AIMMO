using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimeScaleWindow : EditorWindow
{
    float timeScale;
    [MenuItem("MyTools/TimeScaleWindow")]
    private static void ShowWindow()
    {
        TimeScaleWindow window = GetWindow<TimeScaleWindow>();
        window.titleContent = new GUIContent("TimeScaleWindow");
        window.minSize = new Vector2(100, 10);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("DeltaTime: " + timeScale, GUILayout.Width(100));
        timeScale = GUILayout.HorizontalSlider(timeScale, 0, 100, null);
        timeScale = float.Parse(GUILayout.TextField("" + timeScale));
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Set 1"))
        {
            timeScale = 1;
        }
        Time.timeScale = timeScale;
    }
}
