using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public AudioSource audio;
    public void PlayAudio()
    {
        audio.Play();
    }
}
