using System;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Collections.Generic;

public static class ReplaySystem
{
    public static bool isReplaying = false;
    public static float GlobalTime = 0;
    public static int GlobalFrame = 0;
    public static EventHandler FrameHandler;
    public static UnityEvent[] Methods;
    public static string RecordDataPath = "";
    public static void UpdateFrame()
    {
        if (FrameHandler != null)
        {
            FrameHandler(typeof(ReplaySystem), EventArgs.Empty);
        }
    }
    public class FunctionInvokeData
    {
        public int Frame;
        public int Index;
    }

    public class FuctionInvokeDataSet
    {
        public List<string> FunctionInvokeDatas = new List<string>();
    }

    public static FuctionInvokeDataSet GlobalFunctionInvokeDataSet = new FuctionInvokeDataSet();
    public static bool RecordFunctionInvoke(string MethodName)
    {
        if (isReplaying) return false;
        string callName = MethodName;
        int index = 0;
        string result = "null";
        FunctionInvokeData functionInvokeData = new FunctionInvokeData();
        Debug.Log(callName);
        foreach (UnityEvent t in Methods)
        {
            if (callName == Methods[index].GetPersistentMethodName(0))
            {
                functionInvokeData.Frame = ReplaySystem.GlobalFrame;
                functionInvokeData.Index = index;
                result = JsonUtility.ToJson(functionInvokeData);
                GlobalFunctionInvokeDataSet.FunctionInvokeDatas.Add(result);
                File.WriteAllText(ReplaySystem.RecordDataPath + "RecordFunctionInvoke.json", JsonUtility.ToJson(GlobalFunctionInvokeDataSet, true));
                result = callName + ",Index," + index + ",Frame," + ReplaySystem.GlobalFrame + "\n";
                Debug.Log(result);
                return true;
            }
            index++;
        }
        Debug.Log("no function invoke");
        return false;
    }

    public static void Load()
    {
        string content = File.ReadAllText(ReplaySystem.RecordDataPath + "RecordFunctionInvoke.json");
        ReplaySystem.GlobalFunctionInvokeDataSet = JsonUtility.FromJson<ReplaySystem.FuctionInvokeDataSet>(content);
    }

    public static void Play(object sender, EventArgs e)
    {
        if (!isReplaying)
        {
            ReplaySystem.GlobalFrame = 0;
            ReplaySystem.GlobalTime = 0;
            isReplaying = true;
        }
        if (ReplaySystem.GlobalFunctionInvokeDataSet.FunctionInvokeDatas.Count > 0)
        {
            string content = ReplaySystem.GlobalFunctionInvokeDataSet.FunctionInvokeDatas[0];
            ReplaySystem.FunctionInvokeData Data = JsonUtility.FromJson<ReplaySystem.FunctionInvokeData>(content);
            if (Data.Frame == ReplaySystem.GlobalFrame)
            {
                Debug.Log("Do" + Data.Index + "frame : " + Data.Frame);
                Methods[Data.Index].Invoke();
                ReplaySystem.GlobalFunctionInvokeDataSet.FunctionInvokeDatas.RemoveAt(0);
            }
        }
    }

    public static void StartPlay()
    {
        Load();
        FrameHandler += Play;
    }

    public static void StopPlay()
    {
        FrameHandler -= Play;
    }
}
