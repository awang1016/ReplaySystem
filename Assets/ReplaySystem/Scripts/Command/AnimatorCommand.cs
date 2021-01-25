using UnityEngine;
using System;
using System.Collections.Generic;
[SerializeField]
public class AnimatorCommand : IReplayCommand
{
    public int Frame { get; set; }
    Transform Target;
    Animator Animator;
    [Serializable]
    public struct AnimatorData
    {
        public string Name;
        public string Type;
        public string Value;
    }
    public List<AnimatorData> AnimatorDatas = new List<AnimatorData>();
    AnimatorCommand animatorCommand;

    public AnimatorCommand(Animator Animator)
    {
        this.Target = Animator.transform;
        this.Animator = Animator;
    }

    public void Record()
    {
        foreach (AnimatorControllerParameter t in Animator.parameters)
        {
            AnimatorDatas.Add(new AnimatorData
            {
                Name = t.name,
                Type = t.type.ToString(),
                Value = t.type.ToString() == "Bool" ? Animator.GetBool(t.name).ToString() :
                Animator.GetFloat(t.name).ToString()
            });
        }
    }

    public void Play()
    {
        foreach (AnimatorData t in AnimatorDatas)
        {
            if (t.Type == "Bool") Animator.SetBool(t.Name, t.Value == "True" ? true : false);
            else Animator.SetFloat(t.Name, float.Parse(t.Value));
        }
    }

    public string SaveData()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadData(string AnimatorCommandData)
    {
        animatorCommand = JsonUtility.FromJson<AnimatorCommand>(AnimatorCommandData);
        AnimatorDatas = animatorCommand.AnimatorDatas;
    }
}

