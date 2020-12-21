using UnityEngine;

public class TransformCommand : IReplayCommand
{
    public Transform Target;
    TransformCommand transformCommand;
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 localScale;

    public TransformCommand(Transform Target)
    {
        this.Target = Target;
    }
    public void Record()
    {

        this.position = Target.position;
        this.eulerAngles = Target.eulerAngles;
        this.localScale = Target.localScale;
    }

    public void Play()
    {
        Target.position = this.position;
        Target.eulerAngles = this.eulerAngles;
        Target.localScale = this.localScale;
    }

    public string SaveData()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadData(string TransformCommandData)
    {
        transformCommand = JsonUtility.FromJson<TransformCommand>(TransformCommandData);
        this.position = transformCommand.position;
        this.eulerAngles = transformCommand.eulerAngles;
        this.localScale = transformCommand.localScale;
    }
}

