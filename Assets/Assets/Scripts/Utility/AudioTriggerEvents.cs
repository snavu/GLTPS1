using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerEvents : MonoBehaviour
{
    public AudioSource _audioSource;
    [SerializeField] private AudioClip[] voicelines;
    [SerializeField] private AudioClip soundEffect;

    public bool flag1;
    public bool flag2;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FuelingStationVoicelineTrigger") && !flag1)
        {
            if (_audioSource != null && voicelines.Length != 0)
            {
                _audioSource.Stop();
                _audioSource.PlayOneShot(voicelines[0]);
                flag1 = true;
            }
        }
        if (other.gameObject.CompareTag("FuelingStation") && !flag2)
        {
            if (_audioSource != null && voicelines.Length != 0)
            {
                _audioSource.Stop();
                _audioSource.PlayOneShot(voicelines[1]);
                flag2 = true;
            }
        }
        if (other.gameObject.CompareTag("Untagged") ||
            other.gameObject.CompareTag("Metal") ||
            other.gameObject.CompareTag("Pipe") ||
            other.gameObject.CompareTag("Vehicle"))
        {
            if (soundEffect != null)
            {
                _audioSource.PlayOneShot(soundEffect);
            }
        }
    }
}
