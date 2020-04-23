using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    public static AudioClipManager instance;
    public AudioClipHolder clips;
    public float audioCooldown = 1.5F;
    private Dictionary<AudioSource, bool> nonPlayableSources = new Dictionary<AudioSource, bool>();
    private Dictionary<AudioSource, Coroutine> resetCoroutines = new Dictionary<AudioSource, Coroutine>();
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            clips = GetComponent<AudioClipHolder>();
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayClipOneShotWithSource(AudioSource source, AudioClip clip)
    {
        if(source && clip)
        {
            if (!nonPlayableSources.ContainsKey(source))
            {
                nonPlayableSources.Add(source, false);
            }
            if (nonPlayableSources[source] == false)
            {
                source.PlayOneShot(clip);
                nonPlayableSources[source] = true;
               Coroutine c = StartCoroutine(ResetAudioSourcePlayable(source));
                resetCoroutines.Add(source, c);
            }
            else
            {
                return;
            }
        }
      
    }

    private IEnumerator ResetAudioSourcePlayable(AudioSource source)
    {
        yield return new WaitForSeconds(audioCooldown);
        if(source)
        {
            source.Stop();
            nonPlayableSources.Remove(source);
            resetCoroutines.Remove(source);
        }
    }

    public AudioClip GetRandomNoAmmoVL(Player player)
    {
        bool setText = true;
        if (nonPlayableSources.ContainsKey(player.mySource))
        {
            if (nonPlayableSources[player.mySource] == true)
                setText = false;
        }
        int i = Random.Range(1, 4);
        switch (i)
        {
            case 1:
                if (setText)
                    player.SetUxText("Out of ammo!");
                return clips.voiceOutOfAmmo;
            case 2:
                if (setText)
                player.SetUxText("Need to reload!");
                return clips.voiceNeedReload;
            case 3:
                if (setText)
                player.SetUxText("Empy mag!");
                return clips.voiceEmptyMag;
            default:
                Debug.LogError("Clip not found");
                return null;
        }
    }

    public AudioClip GetRandomReloadVL(Player player)
    {
        bool setText = true;
        if (nonPlayableSources.ContainsKey(player.mySource))
        {
            if (nonPlayableSources[player.mySource] == true)
                setText = false;
        }
        int i = Random.Range(1, 3);
        switch (i)
        {
            case 1:
                if(setText)
                player.SetUxText("Swapping mag!");
                return clips.voiceSwappingMag;
            case 2:
                if(setText)
                player.SetUxText("Reloading!");
                return clips.voiceReloading;
            default:
                if(setText)
                Debug.LogError("Clip not found");
                return null;
        }
    }

    public AudioClip GetRandomUltReadyVL(Player player)
    {
        bool setText = true;
        if (nonPlayableSources.ContainsKey(player.mySource))
        {
            if (nonPlayableSources[player.mySource] == true)
                setText = false;
        }
        int i = Random.Range(1, 3);
        switch (i)
        {
            case 1:
                if (setText)
                    player.SetUxText("Ultimate charged!");
                return clips.voiceUltCharged;
            case 2:
                if (setText)
                    player.SetUxText("Ultimate ready!");
                return clips.voiceUltReady;
            default:
                if (setText)
                    Debug.LogError("Clip not found");
                return null;
        }
    }

    public AudioClip GetRandomLowHpVL(Player player)
    {
        bool setText = true;
        if (nonPlayableSources.ContainsKey(player.mySource))
        {
            if (nonPlayableSources[player.mySource] == true)
                setText = false;
        }
        int i = Random.Range(1, 3);
        switch (i)
        {
            case 1:
                if (setText)
                    player.SetUxText("Need healing!");
                return clips.voiceNeedHealing;
            case 2:
                if (setText)
                    player.SetUxText("Need help!");
                return clips.voiceNeedHelp;
            default:
                if (setText)
                    Debug.LogError("Clip not found");
                return null;
        }
    }
    public void HardResetSourcePlayable(AudioSource source)
    {
        source.Stop();
        if (resetCoroutines.ContainsKey(source))
        {
            StopCoroutine(resetCoroutines[source]);
            resetCoroutines.Remove(source);
        }
        if (nonPlayableSources.ContainsKey(source))
        {
            nonPlayableSources.Remove(source);
        }

    }
}
