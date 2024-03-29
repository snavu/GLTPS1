using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnimationEvents : MonoBehaviour
{
    [SerializeField] PlayerInputInitialize playerInputInitializeScript;
    public AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;
    public bool isGroundedConcrete;
    public bool isGroundedMetal;

    public void PlayAudioClipOneShot(AudioClip _audioClip)
    {
        _audioSource.PlayOneShot(_audioClip);
    }
    public void PlayFootstepOneAudioClipOneShot(AnimationEvent animEvent)
    {
        if (animEvent.animatorClipInfo.weight > 0.5f)
        {
            if (isGroundedConcrete)
            {
                _audioSource.PlayOneShot(_audioClip[0]);
            }
            else if (isGroundedMetal)
            {
                _audioSource.PlayOneShot(_audioClip[2]);
            }
        }
    }
    public void PlayFootstepTwoAudioClipOneShot(AnimationEvent animEvent)
    {
        if (animEvent.animatorClipInfo.weight > 0.5f)
        {
            if (isGroundedConcrete)
            {
                _audioSource.PlayOneShot(_audioClip[1]);
            }
            else if (isGroundedMetal)
            {
                _audioSource.PlayOneShot(_audioClip[3]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Metal") ||
         other.gameObject.CompareTag("Pipe") ||
         other.gameObject.CompareTag("Vehicle") ||
         other.gameObject.CompareTag("Elevator"))
        {
            isGroundedMetal = true;
            _audioSource.PlayOneShot(_audioClip[3]);
        }
        else if (other.gameObject.CompareTag("Untagged"))
        {
            isGroundedConcrete = true;
            _audioSource.PlayOneShot(_audioClip[0]);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Metal") ||
                 other.gameObject.CompareTag("Pipe") ||
                 other.gameObject.CompareTag("Vehicle") ||
                 other.gameObject.CompareTag("Elevator"))
        {
            isGroundedMetal = false;
        }
        else if (other.gameObject.CompareTag("Untagged"))
        {
            isGroundedConcrete = false;
        }
    }
}