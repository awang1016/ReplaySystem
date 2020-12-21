using UnityEngine;

public class DemoScriptReplayable : ReplayMethod 
{
    void Start()
    {
        ReplaySystem.Methods = UnityEventList.ToArray();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Fuc1();
        if (Input.GetKeyDown(KeyCode.W)) Fuc2();
        if (Input.GetKeyDown(KeyCode.E)) Fuc3();
        if (Input.GetKeyDown(KeyCode.R)) ReplaySystem.StartPlay();
        if (Input.GetKeyDown(KeyCode.T)) ReplaySystem.StopPlay();
    }

    public void Fuc1()
    {
        ReplaySystem.RecordFunctionInvoke(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("Q");
    }
    public void Fuc2()
    {
        ReplaySystem.RecordFunctionInvoke(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("W");
    }
    public void Fuc3()
    {
        ReplaySystem.RecordFunctionInvoke(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("E");
    }
}
