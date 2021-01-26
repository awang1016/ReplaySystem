using UnityEngine;
using System;

//This code unifies the time and frame of all scripts.
public class ReplayTimer : MonoBehaviour
{
    public FPS _FPS = FPS.FPS30;
    public enum FPS : short
    {
        FPS60 = 60,
        FPS30 = 30,
        FPS10 = 10
    }
    public float _Time = 0;
    public int _Frame = 0;
    public EventHandler TimerHandler;
    float timePerFrame;
    ReplayManager replayManager;

    void Start()
    {
        replayManager = gameObject.GetComponent<ReplayManager>();
        timePerFrame = 1 / (float)_FPS;
        InvokeRepeating("UpdateFrame", 0, timePerFrame);
    }

    public void ResetTimer()
    {
        _Time = 0;
        _Frame = 0;
    }

    public void UpdateFrame()
    {
        ReplaySystem.UpdateFrame();
        ReplaySystem.GlobalFrame = _Frame;
        // ReplaySystem.GlobalTime = _Time;
        if (replayManager._State == Replayable.State.Stop) return;
        // _Time += timePerFrame;
        _Frame++;
    }
    private void Update()
    {
        ReplaySystem.GlobalTime = _Time;
        _Time += Time.deltaTime;
    }
}
