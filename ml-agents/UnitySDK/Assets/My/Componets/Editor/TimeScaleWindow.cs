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
        timeScale = GUILayout.HorizontalSlider(timeScale,0,2,null);
        if(GUILayout.Button("X", GUILayout.Width(25)))
        {
            timeScale = 1;
        }
        GUILayout.EndHorizontal();
        Time.timeScale = timeScale;
    }
}
