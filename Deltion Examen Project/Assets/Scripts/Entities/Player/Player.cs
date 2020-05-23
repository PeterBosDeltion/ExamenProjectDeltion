using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Entity
{
    public delegate void HpEvent();

    public HpEvent zeroTempHp;
    public float lowHealthVoiceThreshold = 300;
    public AudioSource mySource;
    public TextMeshPro uxText;
    private bool waiting;
    private Coroutine resetting;
    public ParticleSystem hitParticle;
    private bool particleUsed;


    protected override void Awake()
    {
        base.Awake();

        hp = maxHp;
        zeroTempHp += EmptyHpEvent;
    }

    protected override void Start()
    {
        base.Start();

        EntityManager.instance.AddPlayerOrAbility(this);

        //mySource = GetComponent<AudioSource>();
        uxText = GetComponentInChildren<TextMeshPro>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        zeroTempHp -= EmptyHpEvent;
    }

    protected override void DamageEvent(Entity Attacker)
    {
        if (tempHp <= 0)
            zeroTempHp.Invoke();
        if (hp <= lowHealthVoiceThreshold)
        {
            AudioClipManager.instance.HardResetSourcePlayable(mySource);
            AudioClipManager.instance.PlayClipOneShotWithSource(mySource, AudioClipManager.instance.GetRandomLowHpVL(this));
        }

        if (!mySource.isPlaying)
            AudioClipManager.instance.PlayClipOneShotWithSource(mySource, AudioClipManager.instance.clips.voiceHurt);

        if(!particleUsed)
        {
            hitParticle.Play();
            particleUsed = true;
            StartCoroutine(HitParticleCooldown());
        }
    }

    public void EmptyHpEvent()
    {
    }

    protected override void Death()
    {
        base.Death();
        GameManager.instance.CheckGameOver();

        if(GameManager.instance.curentState != GameManager.GameState.GameOver && gameObject.activeSelf)
            WipePlayer();
    }

    private void WipePlayer()
    {
        gameObject.SetActive(false);
        MinimapIcon minimapIcon = GetComponentInChildren<MinimapIcon>();
        if (minimapIcon.rendPlayer)
        {
            minimapIcon.SetLineRendererNextAlive();
        }
        //ResetPlayer();
    }

    private void ResetPlayer()
    {
        hp = maxHp;
        //Reset Loadout
        //Reset ability's cooldown & ult charge
     
    }

    public void SetUxText(string newText)
    {
        if (!death)
        {
            if (waiting)
            {
                StopCoroutine(resetting);
                waiting = false;
            }
            uxText.text = newText;
            if (!waiting)
            {
                resetting = StartCoroutine(ResetUxText());
            }
        }
      
    }

    private IEnumerator ResetUxText()
    {
        waiting = true;
        yield return new WaitForSeconds(2);
        uxText.text = "";
        waiting = false;
    }

    private IEnumerator HitParticleCooldown()
    {
        yield return new WaitForSeconds(2);
        particleUsed = false;
    }
}