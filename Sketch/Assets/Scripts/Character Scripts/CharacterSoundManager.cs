using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CharacterSoundManager : MonoBehaviour
{
    #region Variables

    [System.Serializable]
    public class AudioClips
    {
        public AudioClip AirplaneFold;
        public AudioClip Crumple;
        public AudioClip Cut;
        public AudioClip Draw;
        public AudioClip Jump;
        public AudioClip Land;
        public AudioClip Lift;
        public AudioClip TeamUp;
        public AudioClip Uncrumple;
        public AudioClip Walk;
    }
    public AudioClips Audio;

    private AudioSource audioSource;

    #endregion

    #region Initialization

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    #endregion

    #region Play

    public void Play(CharacterAudio clip)
    {
        switch (clip)
        {
            case CharacterAudio.AirplaneFold:
                audioSource.loop = false;
                audioSource.clip = Audio.AirplaneFold;
                audioSource.Play();
                break;
            case CharacterAudio.Crumple:
                audioSource.loop = false;
                audioSource.clip = Audio.Crumple;
                audioSource.Play();
                break;
            case CharacterAudio.Cut:
                audioSource.loop = false;
                audioSource.clip = Audio.Cut;
                audioSource.Play();
                break;
            case CharacterAudio.Draw:
                audioSource.loop = false;
                audioSource.clip = Audio.Draw;
                audioSource.Play();
                break;
            case CharacterAudio.Jump:
                audioSource.loop = false;
                audioSource.clip = Audio.Jump;
                audioSource.Play();
                break;
            case CharacterAudio.Land:
                audioSource.loop = false;
                audioSource.clip = Audio.Land;
                audioSource.Play();
                break;
            case CharacterAudio.Lift:
                audioSource.loop = false;
                audioSource.clip = Audio.Lift;
                audioSource.Play();
                break;
            case CharacterAudio.TeamUp:
                audioSource.loop = false;
                audioSource.clip = Audio.TeamUp;
                audioSource.Play();
                break;
            case CharacterAudio.Uncrumple:
                audioSource.loop = false;
                audioSource.clip = Audio.Uncrumple;
                audioSource.Play();
                break;
            case CharacterAudio.Walk:
                if (!audioSource.clip == Audio.Walk || !audioSource.isPlaying)
                {
                    audioSource.clip = Audio.Walk;
                    audioSource.Play();
                }
                break;
        }
    }

    public void Stop(CharacterAudio clip)
    {
        switch (clip)
        {
            case CharacterAudio.Walk:
                if (audioSource.clip == Audio.Walk && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                break;
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    #endregion
}
