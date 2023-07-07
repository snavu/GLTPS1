using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarNodeAnimationEvents : MonoBehaviour
{
    public AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;
    public int index = 0;

    [SerializeField] private GameObject endScreen;

    public void PlayElevatorSequenceAudioClip()
    {
        _audioSource.PlayOneShot(_audioClip[index]);
        index++;
    }

    public void End()
    {

    }




}
