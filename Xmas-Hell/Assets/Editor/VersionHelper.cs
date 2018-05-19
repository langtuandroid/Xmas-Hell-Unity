using System;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

class VersionHelper : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
#if UNITY_EDITOR
        setBuildProperties();
#endif
    }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
#if UNITY_EDITOR
        revertBuildPropertiesToDefault();
#endif
    }

    public static void setBuildProperties()
    {
        DateTime currdate = DateTime.Now;
        string versionAppend = "";
        string gitVersion = GetCommitId();
        if (gitVersion.Length > 0)
            versionAppend = " (" + gitVersion + ")";

        // Package Name
        foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)).Cast<BuildTargetGroup>())
            PlayerSettings.SetApplicationIdentifier(group, "io.noxalus");

        // Package Version
        PlayerSettings.bundleVersion = "io.noxalus.XmasHell." + currdate.ToString("yy.MM.dd") + versionAppend;
        PlayerSettings.iOS.buildNumber = gitVersion;
        PlayerSettings.Android.bundleVersionCode = int.Parse(gitVersion);
    }

    public static void revertBuildPropertiesToDefault()
    {
        PlayerSettings.bundleVersion = "io.noxalus.unknown";
        PlayerSettings.iOS.buildNumber = "0";
        PlayerSettings.Android.bundleVersionCode = 1;
    }

    // by https://stackoverflow.com/q/26515656
    public static string GetCommitId()
    {
        string strCommit = "";

        Process p = new Process();
        // Set path to git exe.
        p.StartInfo.FileName = "git";
        // Set git command.
        p.StartInfo.Arguments = "rev-parse --short HEAD";
        // Set working directory.
        p.StartInfo.WorkingDirectory = Application.dataPath + "/../../";
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.UseShellExecute = false;
        p.Start();

        // Pass output to variable.
        strCommit = p.StandardOutput.ReadToEnd();

        p.WaitForExit();

        if (string.IsNullOrEmpty(strCommit) == true)
        {
            UnityEngine.Debug.LogError("UNABLE TO COMMIT HASH");
        }

        strCommit = strCommit.Trim();

        return strCommit;
    }

    public void OnPreprocessBuild(BuildReport report)
    {
    }
}