using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class ReplayAnimator : Replayable
{
    ReplayInvoker replayInvoker;
    Animator animator;
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        replayInvoker = new ReplayInvoker(gameObject.name, animator);
        ReplaySystem.FrameHandler += ReplaySwitch;
    }

    void ReplaySwitch(object sender, EventArgs e)
    {
        switch (_State)
        {
            case State.Live:
                break;
            case State.Record:
                replayInvoker.RecordCommand(new AnimatorCommand(animator));
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
        replayInvoker.SaveAnimatorData();
    }

    public override void LoadData()
    {
        replayInvoker.LoadAnimatorData();
    }
}
