using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Entity
{
    public delegate void HpEvent();
    public delegate void ReviveEvent();
    public ReviveEvent reviveEvent;
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
        reviveEvent += EmptyHpEvent;
        reviveEvent += ResetPlayer;
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
        reviveEvent -= EmptyHpEvent;
        reviveEvent -= ResetPlayer;
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

        if(!particleUsed && gameObject.activeSelf)
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
        {
            WipePlayer();
            if(!LevelManager.instance.playerDowned)
                LevelManager.instance.playerDowned = true;
        }
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
        PlayerController controller = GetComponent<PlayerController>();
        controller.currentPrimary.FullResetValues();
        controller.currentSecondary.FullResetValues();
        controller.ultimateAbility.currentUltCharge = 0;
        controller.ultimateAbility.checkUltCharged = false;
        controller.ultimateAbility.active = false;
        controller.ultimateAbility.ultActive = false;
        controller.ultimateAbility.onCooldown = false;
        controller.ultimateAbility.deploying = false;

        foreach (Ability a in controller.abilities)
        {
            a.active = false;
            a.onCooldown = false;
            a.deploying = false;
        }

        death = false;
        SetUxText("");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AudioClipManager.instance.PlayClipOneShotWithSource(mySource, AudioClipManager.instance.clips.voiceAffirmative);
            SetUxText("Affirmative!");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AudioClipManager.instance.PlayClipOneShotWithSource(mySource, AudioClipManager.instance.clips.voiceNegative);
            SetUxText("Negative!");
        }
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