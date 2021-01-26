using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Record and Save voice.
public class ReplayVoice : Replayable
{
    public AudioClip LoadClip;
    public AudioSource AudioSource;
    bool isRecording = false;
    int Hz = 44100;
    List<AudioClip> ListAudioClip = new List<AudioClip>();
    AudioClip FinalClip;

    IEnumerator Record()
    {
        while (true)
        {
            AudioSource.clip = Microphone.Start(null, true, 10, Hz);
            ListAudioClip.Add(AudioSource.clip);
            yield return new WaitForSeconds(10);
        }
    }

    private void Update()
    {
        ReplaySwitch();
    }

    void ReplaySwitch()
    {
        switch (_State)
        {
            case State.Live:
                isRecording = false;
                break;
            case State.Record:
                if (!isRecording)
                {
                    isRecording = true;
                    StartRecord();
                }
                break;
            case State.Rewind:
                isRecording = false;
                Play(ReplaySystem.GlobalTime);
                break;
            case State.Play:
                isRecording = false;
                Play(ReplaySystem.GlobalTime);
                break;
            case State.Stop:
                isRecording = false;
                break;
            default:
                break;
        }
    }

    public void StartRecord()
    {
        StartCoroutine("Record");
    }

    public override void SaveData()
    {
        Microphone.End("");
        StopCoroutine("Record");
        MergeAudioClips(ListAudioClip);
        SaveWavFile();
    }

    public override void LoadData()
    {
        LoadClip = WavUtility.ToAudioClip();
        AudioSource.clip = LoadClip;
    }

    void MergeAudioClips(List<AudioClip> Clip)
    {
        List<float[]> Data = new List<float[]>();
        List<float> ar = new List<float>();
        int i = 0;
        foreach (AudioClip t in Clip)
        {
            Data.Add(new float[t.samples * t.channels]);
            t.GetData(Data[i], 0);
            ar.AddRange(Data[i]);
            i++;
        }
        float[] datas = ar.ToArray();
        FinalClip = AudioClip.Create("FinalClip", datas.Length, 1, Hz, false);
        FinalClip.SetData(datas, 0);
    }

    public string SaveWavFile()
    {
        string filepath;
        byte[] bytes = WavUtility.FromAudioClip(FinalClip, out filepath, true);
        Debug.Log(filepath);
        return filepath;
    }

    public void Play(float playtime)
    {
        if (!LoadClip) return;
        if (playtime > LoadClip.length || playtime < 0) return;
        AudioSource.time = playtime;
        if (!AudioSource.isPlaying) AudioSource.Play();
    }

}