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

        mySource = GetComponent<AudioSource>();
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
    }

    public void EmptyHpEvent()
    {
    }

    protected override void Death()
    {
        
    }

    public void SetUxText(string newText)
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

    private IEnumerator ResetUxText()
    {
        waiting = true;
        yield return new WaitForSeconds(2);
        uxText.text = "";
        waiting = false;
    }
}