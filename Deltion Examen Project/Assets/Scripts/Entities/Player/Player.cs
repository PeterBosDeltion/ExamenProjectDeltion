using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Player : Entity
{
    public delegate void HpEvent();

    public HpEvent zeroTempHp;
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

    private void Start()
    {
        EntityManager.instance.AddPlayerOrAbility(this);

        mySource = GetComponent<AudioSource>();
        uxText = GetComponentInChildren<TextMeshPro>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        zeroTempHp -= EmptyHpEvent;
    }

    public override void DamageEvent(Entity Attacker)
    {
        if (tempHp <= 0)
            zeroTempHp.Invoke();
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