using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSoundIn : MonoBehaviour
{
    private AudioSource audioSource;
    public float targetVolume;
    public float timeToFade;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;

        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / timeToFade;

            yield return null;
        }
    }
}