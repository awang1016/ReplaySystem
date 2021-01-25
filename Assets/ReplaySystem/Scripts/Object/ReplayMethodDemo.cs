using UnityEngine;

[RequireComponent(typeof(ReplayInvoker))]
public class ReplayMethodDemo : ReplayMethod
{
    ReplayInvoker replayInvoker;
    MethodCommand abc;
    void Start()
    {
        replayInvoker = gameObject.GetComponent<ReplayInvoker>();
        // ReplaySystem.Methods = UnityEventList.ToArray();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Fuc1();
        if (Input.GetKeyDown(KeyCode.W)) Fuc2();
        if (Input.GetKeyDown(KeyCode.E)) Fuc3();
        // if (_State == State.Play) replayInvoker.PlayMethodCommand();
        // if (Input.GetKeyDown(KeyCode.T)) ReplaySystem.StopPlay();
    }

    public void Fuc1()
    {
        replayInvoker.RecordCommand(new MethodCommand(Fuc1));
        // ReplaySystem.RecordFunctionInvoke(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("Q");
    }
    public void Fuc2()
    {
        replayInvoker.RecordCommand(new MethodCommand(Fuc2));
        // ReplaySystem.RecordFunctionInvoke(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("W");
    }
    public void Fuc3()
    {
        replayInvoker.RecordCommand(new MethodCommand(Fuc3));
        // ReplaySystem.RecordFunctionInvoke(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("E");
    }
}
