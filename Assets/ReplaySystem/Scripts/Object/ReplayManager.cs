using UnityEngine;
using System.Collections.Generic;
using System;

//Control all Replayable script.
[RequireComponent(typeof(ReplayTimer))]
public class ReplayManager : Replayable
{
    public string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
    public string FolderName = "Default";
    List<Replayable> ReplayList = new List<Replayable>();
    State LastState;
    ReplayTimer replayTimer;

    void Awake()
    {
        ReplaySystem.RecordDataPath = Path + FolderName + "\\";
        replayTimer = gameObject.GetComponent<ReplayTimer>();
        foreach (Replayable temp in Resources.FindObjectsOfTypeAll<Replayable>())
        {
            if (temp != this) ReplayList.Add(temp);
        }
        ChangeState(State.Live);
    }

    void ChangeState(State mState)
    {
        foreach (Replayable t in ReplayList)
        {
            if (t) t._State = mState;
        }
        replayTimer.ResetTimer();
        LastState = mState;
    }

    void SaveAllData()
    {
        foreach (Replayable t in ReplayList)
        {
            if (t)
            {
                t.SaveData();
            }
        }
    }

    void LoadAllData()
    {
        foreach (Replayable t in ReplayList)
        {
            if (t)
            {
                t.LoadData();
            }
        }
    }

    void Update()
    {
        if (LastState != _State)
        {
            ChangeState(_State);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _State = State.Live;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _State = State.Record;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _State = State.Play;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveAllData();
            Debug.Log("SaveData");
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            LoadAllData();
            Debug.Log("LoadData");
        }
    }
}
