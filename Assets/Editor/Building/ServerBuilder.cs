using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class ServerBuilder
{
    private static Process _serverProcess;
    private static ManualResetEvent _waitForServerToExitEvent;
    public static bool Build(BuildOptions options)
    {
        if (_serverProcess != null)
        {
            Debug.Log("Server is running, killing the running server");

            KillRunningServer();

            _waitForServerToExitEvent.WaitOne(500);
        }

        Debug.Log("Starting server build");

        string path = Application.dataPath.Replace("Assets", "") + "build/server";
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        else
        {
            FileInfo[] files = new DirectoryInfo(path).GetFiles("*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                File.Delete(file.FullName);
            }
        }

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        List<string> scenePaths = new List<string>();
        foreach (var scene in scenes)
        {
            if (scene.enabled && scene.path.ToLower().Contains("SceneBuildNetworking"))
                scenePaths.Add(scene.path);
        }

        EditorUserBuildSettings.enableHeadlessMode = false;

        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        buildPlayerOptions.scenes = scenePaths.ToArray();
        buildPlayerOptions.options = options;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.locationPathName = path + "/server.exe";

        Debug.Log("Building server with: " + scenePaths.Count + " scenes");

        BuildReport br = BuildPipeline.BuildPlayer(buildPlayerOptions);

        return br.summary.result == BuildResult.Succeeded;
    }

    public static void StartLatestBuild()
    {
        KillRunningServer();

        Process p = new Process();
        p.StartInfo.FileName = Application.dataPath.Replace("Assets", "") + "build/server/server.exe";
        p.EnableRaisingEvents = true;
        p.Exited += (o, e) =>
        {
            Debug.Log("Server exited");
            _serverProcess = null;
            _waitForServerToExitEvent.Set();
        };

        p.Start();

        _serverProcess = p;
    }

    private static void KillRunningServer()
    {
        if (_serverProcess != null)
        {
            Debug.Log("Stopping running server");
            try
            {
                if(_waitForServerToExitEvent == null)
                    _waitForServerToExitEvent = new ManualResetEvent(false);

                _waitForServerToExitEvent.Reset();
                _serverProcess.Kill();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            finally
            {
                _serverProcess = null;
            }
        }
    }

    public static bool BuildExists()
    {
        return File.Exists(Application.dataPath.Replace("Assets", "") + "build/server/server.exe");
    }
}
