using UnityEngine;
using System.IO;

public class MethodCommand : IReplayCommand
{
    public delegate void Method();
    public int Frame { get; set; }
    public Method mMethod;
    public MethodCommand(Method mMethod)
    {
        this.mMethod += mMethod;
    }
    public void Record()
    {
        Frame = ReplaySystem.GlobalFrame;
    }
    public void Play()
    {
        this.mMethod.Invoke();
    }
    public string SaveData() { return JsonUtility.ToJson(this, true); }
    public void LoadData(string Data) { }
}
