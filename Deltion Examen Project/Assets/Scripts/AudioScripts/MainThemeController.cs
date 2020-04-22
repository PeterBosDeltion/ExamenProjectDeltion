using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThemeController : MonoBehaviour
{
    [Tooltip("If false themes will circulate around the list linearly")]
    public bool random;
    public float targetVolume;
    public float timeToFade;
    public float timeBetweenClips;

    public List<AudioClip> themes = new List<AudioClip>();
    private AudioSource audioSource;

    private int CurrentIndex;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateMusicClip(0);
    }

    void UpdateMusicClip(int index = 0)
    {
        CurrentIndex = index;

        audioSource.Stop();

        if (random)
        {
            audioSource.clip = themes[Random.Range(0, themes.Count - 1)];
        }
        else
        {
            audioSource.clip = themes[index];
        }

        audioSource.Play();

        StartCoroutine(FadeInAndOut(audioSource.clip));
    }

    private IEnumerator FadeInAndOut(AudioClip clip)
    {
        audioSource.volume = 0;

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / timeToFade;

            yield return null;
        }

        yield return new WaitForSeconds(clip.length - (timeToFade * 2));

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume -= targetVolume * Time.deltaTime / timeToFade;

            yield return null;
        }

        yield return new WaitForSeconds(timeBetweenClips);

        if (CurrentIndex != themes.Count - 1)
            UpdateMusicClip(CurrentIndex += 1);
        else
            UpdateMusicClip(0);
    }
}
