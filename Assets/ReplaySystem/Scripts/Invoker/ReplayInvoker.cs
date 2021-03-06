﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ReplayInvoker
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
    string name;
    Transform transform;
    Animator animator;

    public ReplayInvoker(string name, Animator animator)
    {
        this.animator = animator;
        init(name);
    }

    public ReplayInvoker(string name, Transform transform)
    {
        this.transform = transform;
        init(name);
    }

    void init(string name)
    {
        this.name = name;
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

    public void PlayCommand()
    {
        if (ReplaySystem.GlobalFrame <= 0) return;
        if (commandHistory.Count > ReplaySystem.GlobalFrame) commandHistory[ReplaySystem.GlobalFrame].Play();
    }

    public void RecordCommand(IReplayCommand command)
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
        foreach (IReplayCommand c in commandHistory)
        {
            Lines.Add(c.SaveData());
        }
        System.IO.File.WriteAllLines(ReplaySystem.RecordDataPath + $"/{name}.json", Lines);
    }

    public void SaveAnimatorData()
    {
        if (!Directory.Exists(ReplaySystem.RecordDataPath))
        {
            DirectoryInfo di = Directory.CreateDirectory(ReplaySystem.RecordDataPath);
        }
        List<string> Lines = new List<string>();
        foreach (IReplayCommand c in commandHistory)
        {
            Lines.Add(c.SaveData());
        }
        System.IO.File.WriteAllLines(ReplaySystem.RecordDataPath + $"/{name}_Animator.json", Lines);
    }

    public void LoadAnimatorData()
    {
        commandHistory.Clear();
        string[] Lines = System.IO.File.ReadAllLines(ReplaySystem.RecordDataPath + $"/{name}_Animator.json");
        foreach (string str in Lines)
        {
            IReplayCommand c = new AnimatorCommand(animator);
            c.LoadData(str);
            commandHistory.Add(c);
        }
    }

    public void LoadTransformData()
    {
        commandHistory.Clear();
        string[] Lines = System.IO.File.ReadAllLines(ReplaySystem.RecordDataPath + $"/{name}.json");
        foreach (string str in Lines)
        {
            TransformCommand c = new TransformCommand(transform);
            c.LoadData(str);
            commandHistory.Add(c);
        }
    }
}
