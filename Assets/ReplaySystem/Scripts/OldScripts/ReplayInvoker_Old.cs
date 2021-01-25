using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ReplayInvoker_Old : MonoBehaviour
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

    public void PlayCommand()
    {
        if (ReplaySystem.GlobalFrame <= 0) return;
        if (commandHistory.Count > ReplaySystem.GlobalFrame) commandHistory[ReplaySystem.GlobalFrame].Play();
    }
    int i = 0;
    public delegate void Methods();
    Methods methods;
    List<IReplayCommand> commadMethodList = new List<IReplayCommand>();
    public void PlayMethodCommand()
    {
        methods = null;
        if (ReplaySystem.GlobalFrame <= 0) return;
        if (ReplaySystem.GlobalFrame == commandHistory[i].Frame)
        {
            methods += PlayCommand;
            i++;
        }
    }
    // ReplayMethod wait fix 1221       
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
        System.IO.File.WriteAllLines(ReplaySystem.RecordDataPath + $"/{gameObject.name}.json", Lines);
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
        System.IO.File.WriteAllLines(ReplaySystem.RecordDataPath + $"/{gameObject.name}_Animator.json", Lines);
    }
    public void LoadAnimatorData()
    {
        commandHistory.Clear();

        string[] Lines = System.IO.File.ReadAllLines(ReplaySystem.RecordDataPath + $"/{gameObject.name}_Animator.json");
        foreach (string str in Lines)
        {
            // IReplayCommand c = new AnimatorCommand();
            // c.LoadData(str);
            // commandHistory.Add(c);
        }
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
