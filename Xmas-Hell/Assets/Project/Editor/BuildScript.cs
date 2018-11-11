using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class BuildScript
{
    [MenuItem("File/Builder/Android")]
    static void PerformAndroidBuild()
    {
        string[] scenes = {
            "Assets/Scenes/main.unity"
        };

        string[] commandLineArguments = Environment.GetCommandLineArgs();
        List<string> buildArguments = new List<string>();
        bool areBuildArguments = false;
        foreach (var argument in commandLineArguments)
        {
            if (areBuildArguments)
            {
                if (argument.StartsWith("-"))
                    break;
                else
                {
                    buildArguments.Add(argument);
                }
            }
            else if (argument == "-executeMethod")
                areBuildArguments = true;
        }

        Debug.Log("Build arguments: " + string.Join(" | ", buildArguments));

        string buildPath = "Builds/Android/Xmas-Hell.apk";

        // We assume the first argument is the static method to call
        // and the second argument is the build output path
        if (buildArguments.Count > 1)
            buildPath = buildArguments[1];

        // Create build folder if not yet exists
        DirectoryInfo dirInfo;
        if (!Directory.Exists(buildPath))
            dirInfo = new DirectoryInfo(buildPath);
        else
            dirInfo = Directory.CreateDirectory(buildPath);

        Debug.Log("Output directory: " + dirInfo.FullName);

        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.Development);
    }
}