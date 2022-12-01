using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [SerializeField] private List<AudioClip> clips;
    [SerializeField] private AudioSource speaker;

    void Awake()
    {
        Instance = this;
    }

    public void PlayClip(int id, float vol = 1, bool pitchVar = false)
    {
        if(pitchVar) { speaker.pitch = Random.Range(0.9f, 1.1f); }
        speaker.volume = vol;
        speaker.PlayOneShot(clips[id]);
    }
}
