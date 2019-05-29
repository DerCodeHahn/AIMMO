using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    bool showTimeScaleUI;
    float timeScale;
    private void OnGUI()
    {
        showTimeScaleUI = GUILayout.Toggle(showTimeScaleUI, "TimeScaling");
        if (showTimeScaleUI)
        {
            GUILayout.Label("DeltaTime: " + timeScale, GUILayout.Width(200));
            timeScale = GUILayout.HorizontalSlider(timeScale, 0, 100, null);
            timeScale = float.Parse(GUILayout.TextField("" + timeScale));
            Time.timeScale = timeScale;
        }
    }
}
