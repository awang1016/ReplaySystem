using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Record and Save voice.
public class RecordVoice : MonoBehaviour
{

    public AudioClip LoadClip;
    public AudioSource AudioSource;
    public int RecordTime = 0;
    public float time = 0;
    int Hz = 44100;
    // bool isRecord = true;

    List<AudioClip> ListAudioClip = new List<AudioClip>();
    // AudioSource source;
    // AudioClip clip;
    AudioClip FinalClip;

    IEnumerator Record()
    {
        while (true)
        {
            AudioSource.clip = Microphone.Start(null, true, 1, Hz);
            ListAudioClip.Add(AudioSource.clip);
            RecordTime++;
            yield return new WaitForSeconds(1);
        }
    }

    public void StartRecord()
    {
        StartCoroutine("Record");
    }
    public void SaveRecord()
    {
        Microphone.End("");
        StopCoroutine("Record");
        MergeAudioClips(ListAudioClip);
        SaveWavFile();
    }
    public string SaveWavFile()
    {
        string filepath;
        byte[] bytes = WavUtility.FromAudioClip(FinalClip, out filepath, true);
        return filepath;
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
    public void LoadRecord()
    {
        LoadClip = WavUtility.ToAudioClip();
        AudioSource.clip = LoadClip;
    }
    public void PlayRecord(float playtime)
    {
        AudioSource.time = playtime;
        if (!AudioSource.isPlaying) AudioSource.Play();
    }
    public void PlayOneShot()
    {
        // AudioSource.Play();
        // while (AudioSource.isPlaying)
        // {
        //     playTime += Time.deltaTime;
        //     AudioSource.time = playTime;
        // }
    }
    // private void OnDestroy()
    // {
    //     SaveRecord();
    // }
    // void OnGUI()
    // {

    //     if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - -350, 200, 100), "Time"))
    //     {
    //         PlayOneShot();
    //     }
    //     if (isRecord)
    //     {
    //         if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 250, 200, 100), "Record"))
    //         {
    //             isRecord = !isRecord;
    //             StartRecord();
    //         }
    //     }
    //     else
    //     {
    //         if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 250, 200, 100), "SaveRecord"))
    //         {
    //             SaveRecord();
    //         }
    //     }
    //     if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 100), "LoadRecord"))
    //     {

    //         LoadRecord();
    //     }
    // }
}