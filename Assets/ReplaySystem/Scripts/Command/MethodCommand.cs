using UnityEngine;
using System.IO;

public class MethodCommand : IReplayCommand
{
    public int Frame = 0;
    public string Name;
    public int Index;
    public MethodCommand(string Name)
    {
        this.Name = Name;
        this.Frame = ReplaySystem.GlobalFrame;
    }
    public void Record() { }
    public void Play()
    {
        File.AppendAllText(ReplaySystem.RecordDataPath + Name, JsonUtility.ToJson(this, true));
    }
    public string SaveData() { return JsonUtility.ToJson(this, true); }
    public void LoadData(string Data) { }
}
