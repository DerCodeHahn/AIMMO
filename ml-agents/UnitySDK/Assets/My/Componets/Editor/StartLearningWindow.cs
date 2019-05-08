
using UnityEngine;
using UnityEditor;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class StartLearningWindow : EditorWindow
{
    StreamWriter messageStream;
    Process process;
    String lastLine;
    void ExeCommand(string command)
    {

        process = new System.Diagnostics.Process();
        process.EnableRaisingEvents = true;
        process.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardError = true;
        process.OutputDataReceived += new DataReceivedEventHandler(DataReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        // process.StartInfo.Arguments = "-c \"activate ml-agents\n\"";
        process.Start();
        process.BeginOutputReadLine();
        messageStream = process.StandardInput;
        //proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

        //process.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
        // ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Windows\\system32\\cmd.exe");
        // //startInfo.WorkingDirectory = "C:";
        // //startInfo.UseShellExecute = false;
        // startInfo.RedirectStandardInput = true;
        // startInfo.RedirectStandardOutput = true;
        // process.StartInfo = startInfo;
        // process.StartInfo.RedirectStandardError = true;
        // process.EnableRaisingEvents = false;
        // process.StartInfo.UseShellExecute = false;
        
        // process.OutputDataReceived += new DataReceivedEventHandler(DataReceived);
        // process.ErrorDataReceived += new DataReceivedEventHandler(ErrorReceived);
        // // process.StartInfo.UseShellExecute = false;
        // process.StartInfo.RedirectStandardOutput = true;
        // process.StartInfo.RedirectStandardInput = true;
        // process.Start();
        // process.BeginOutputReadLine();
        messageStream.Write("activate ml-agents\n");
        messageStream.Flush();
        messageStream.Write("mlagents-learn config/trainer_config.yaml --run-id=" + learningName + " --train \n");
        messageStream.Flush();
        // process.WaitForExit();
        // while (!process.StandardOutput.EndOfStream)
        // {
        //     UnityEngine.Debug.Log(process.StandardOutput.ReadLine());
        // }

    }

    private void ErrorReceived(object sender, DataReceivedEventArgs e)
    {
        UnityEngine.Debug.LogError(e.Data);
    }

    private void DataReceived(object sender, DataReceivedEventArgs e)
    {
        UnityEngine.Debug.Log(e.Data);
        lastLine = e.Data;
    }




    // if( process != null  !process.HasExited )
    // {
    //     process.Kill();
    // }



    // float refreshrate;
    // IEnumerator Listen(Process process)
    // {
    //     yield return new WaitForSeconds(refreshrate);
    //     string line = process.StandardOutput.ReadLine();
    //     while (line != null)
    //     {
    //         UnityEngine.Debug.Log("line:" + line);
    //         line = process.StandardOutput.ReadLine();
    //     }
    // }

    [MenuItem("MyTools/StartLearningWindow")]
    private static void ShowWindow()
    {
        var window = GetWindow<StartLearningWindow>();
        window.titleContent = new GUIContent("StartLearningWindow");
        window.minSize = new Vector2(100, 10);
        window.Show();
    }
    string learningName = "";
    bool load = false;
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        learningName = GUILayout.TextField(learningName);
        load = GUILayout.Toggle(load, "Continue Learning", GUILayout.Width(150));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Start Learning Interface"))
        {
            ExeCommand(learningName);
        }
        GUILayout.Label(lastLine);
    }
}

