using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public static class IdeaLogger
{
    private static string logFolderPath;
    private static string logFileName;

    //private static string gameStartTimeString;
    private static bool startedLogging;

    public static void StartLogging(string topic)
    {
        logFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Game-Storming/Idea-logs";
        string gameStartTimeString = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt").Replace('/', '-').Replace('\\', '-').Replace(' ', '-').Replace(':', '-');
        logFileName = gameStartTimeString;

        startedLogging = true;

        StringBuilder sb = new StringBuilder();

        startedLogging = true;
        sb.Append("Game started at: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt"));
        sb.Append('\r');

        sb.Append("Topic:," + topic);
        sb.Append('\r');

        WriteToFile(logFolderPath, logFileName, Encoding.Unicode.GetBytes(sb.ToString()));
    }

    public static void LogIdea(Participant p, string idea)
    {
        if(!startedLogging)
            return;

        StringBuilder sb = new StringBuilder();

        //if (!startedLogging)
        //{
        //    startedLogging = true;
        //    sb.Append("Game started at: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt"));
        //    sb.Append('\r');
            
        //    sb.Append("Topic:," + topic);
        //    sb.Append('\r');
        //}

        sb.Append(p.Name);
        sb.Append(',');
        sb.Append(idea);
        sb.Append(',');
        sb.Append(DateTime.Now.ToString("h:mm:ss"));
        sb.Append('\r');

        WriteToFile(logFolderPath, logFileName, Encoding.Unicode.GetBytes(sb.ToString()));
    }

    public static void EndLogging()
    {
        if(startedLogging)
            WriteToFile(logFolderPath, logFileName, Encoding.Unicode.GetBytes("Game ended at: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt")));
    }

    private static void WriteToFile(string folder, string filename, byte[] data)
    {
        string filePath = folder + '/' + filename + ".csv";

        FileInfo fi = new FileInfo(filePath);

        FileStream fs = null;

        if (!fi.Exists && fi.Directory != null)
        {
            Directory.CreateDirectory(fi.Directory.FullName);
            fs = File.Create(filePath);
        }
        else if (fi.Exists)
        {
            fs = File.Open(filePath, FileMode.Append);
        }

        if (fs != null)
        {
            fs.Write(data, 0, data.Length);
            fs.Flush(true);
            fs.Dispose();
        }
    }
}
