using UnityEngine;

public class SoundService : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

  
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f)
    {
        if (clip == null) return;

        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = loop;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
}