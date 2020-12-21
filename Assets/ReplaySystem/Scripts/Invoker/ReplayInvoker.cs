using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ReplayInvoker : MonoBehaviour
{
    public Queue<IReplayCommand> commandBuffer;
    public List<IReplayCommand> commandHistory;
    public int HistoryLength
    {
        get
        {
            return commandHistory.Count - 1;
        }
    }
    int counter = 0;

    void Awake()
    {
        commandBuffer = new Queue<IReplayCommand>();
        commandHistory = new List<IReplayCommand>();
        ReplaySystem.FrameHandler += delegate (object a, EventArgs e)
        {
            if (commandBuffer.Count > 0)
            {
                IReplayCommand command = commandBuffer.Dequeue();
                command.Record();
                commandHistory.Add(command);
                counter++;
            }
        };
    }

    public void PlayCommand(TransformCommand command)
    {
        if (ReplaySystem.GlobalFrame <= 0) return;
        if (commandHistory.Count > ReplaySystem.GlobalFrame) commandHistory[ReplaySystem.GlobalFrame].Play();
    }

    public void RecordCommand(TransformCommand command)
    {
        while (commandHistory.Count > counter)
        {
            commandHistory.RemoveAt(counter);
        }
        commandBuffer.Enqueue(command);
        commandHistory.Add(command);
    }

    public void SaveTransformData()
    {
        if (!Directory.Exists(ReplaySystem.RecordDataPath))
        {
            DirectoryInfo di = Directory.CreateDirectory(ReplaySystem.RecordDataPath);
        }
        List<string> Lines = new List<string>();
        foreach (TransformCommand c in commandHistory)
        {
            Lines.Add(c.SaveData());
        }
        System.IO.File.WriteAllLines(ReplaySystem.RecordDataPath + $"/{gameObject.name}.json", Lines);
    }

    public void LoadTransformData()
    {
        commandHistory.Clear();

        string[] Lines = System.IO.File.ReadAllLines(ReplaySystem.RecordDataPath + $"/{gameObject.name}.json");
        foreach (string str in Lines)
        {
            TransformCommand c = new TransformCommand(transform);
            c.LoadData(str);
            commandHistory.Add(c);
        }
    }
}
