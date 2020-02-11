using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Gun", menuName = "Weapon", order = 1)]
public class WeaponScriptable : ScriptableObject
{
    public float requiredLevel;
    public float minFallOff;
    public float maxFallOff;
    public float projectileVelocity;
    public float damage;
    public float firerate;
    public float totalAmmo;
    public float ammoDrain;
    //public enum FireType
    //{
    //    Auto,
    //    Semi,
    //    Bolt,
    //}

    //public FireType myFireType;
    public float reloadSpeed;
}
