using System;
using UnityEngine;

//This script that can add the Command to Invoker to record,play,save,load data.
[RequireComponent(typeof(ReplayInvoker))]
public class ReplayTransform : Replayable
{
    ReplayInvoker replayInvoker;

    void Awake()
    {
        replayInvoker = gameObject.GetComponent<ReplayInvoker>();
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
                replayInvoker.PlayCommand(new TransformCommand(transform));
                break;
            case State.Stop:
                replayInvoker.PlayCommand(new TransformCommand(transform));
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