public interface IReplayCommand
{
    void Record();
    void Play();
    string SaveData();
    void LoadData(string data);
}
