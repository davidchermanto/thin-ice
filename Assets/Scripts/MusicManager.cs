using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private List<AudioSource> speakers;

    void Awake()
    {
        Instance = this;
    }

    public void Play(int id)
    {
        ShutOff();
        speakers[id].volume = 1;
    }

    public void ShutOff()
    {
        foreach (AudioSource speaker in speakers)
        {
            speaker.volume = 0;
        }
    }

}
