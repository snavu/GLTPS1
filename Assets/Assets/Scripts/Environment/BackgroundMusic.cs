using UnityEngine;

public class PlaylistPlayer : MonoBehaviour
{
    public AudioClip[] playlist; // Array of audio clips for the playlist
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayTrack(currentTrackIndex);
    }

    // Play the specified track from the playlist
    void PlayTrack(int trackIndex)
    {
        audioSource.clip = playlist[trackIndex];
        audioSource.Play();
    }

    void Update()
    {
        // Check if the current track has finished playing
        if (!audioSource.isPlaying)
        {
            // Move to the next track in the playlist
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
            // Play the next track
            PlayTrack(currentTrackIndex);
        }
    }
}
