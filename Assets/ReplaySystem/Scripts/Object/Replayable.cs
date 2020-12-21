//This abstract class is the basic class of all replayabe objects
public abstract class Replayable : UnityEngine.MonoBehaviour
{
    public enum State
    {
        Live,
        Record,
        Play,
        Rewind,
        Stop
    }
    public State _State = State.Live;
    public virtual void SaveData()
    {

    }
    public virtual void LoadData()
    {

    }
}
