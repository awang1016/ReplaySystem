public interface IReplayCommand
{
    int Frame { get; set; }
    void Record();
    void Play();
    string SaveData();
    void LoadData(string data);
}
