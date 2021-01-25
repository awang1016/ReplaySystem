using System;
using UnityEngine;

//This script that can add the Command to Invoker to record,play,save,load data.
public class ReplayTransform : Replayable
{
    ReplayInvoker replayInvoker;

    void Awake()
    {
        replayInvoker = new ReplayInvoker(gameObject.name, transform);
        ReplaySystem.FrameHandler += ReplaySwitch;
    }

    void ReplaySwitch(object sender, EventArgs e)
    {
        switch (_State)
        {
            case State.Live:
                break;
            case State.Record:
                replayInvoker.RecordCommand(new TransformCommand(transform));
                break;
            case State.Rewind:
                break;
            case State.Play:
                replayInvoker.PlayCommand();
                break;
            case State.Stop:
                replayInvoker.PlayCommand();
                break;
            default:
                break;
        }
    }

    public override void SaveData()
    {
        replayInvoker.SaveTransformData();
    }

    public override void LoadData()
    {
        replayInvoker.LoadTransformData();
    }
}