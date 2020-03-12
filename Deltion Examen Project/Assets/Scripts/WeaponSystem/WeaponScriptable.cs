using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Gun", menuName = "Gameplay/Weapon", order = 1)]
public class WeaponScriptable : ScriptableObject
{
    public new string name;
    [TextArea(15, 20)]
    public string description;
    public float requiredLevel;
    public float minFallOff;
    public float maxFallOff;
    public float projectileVelocity;
    public float damage;
    public float firerate;
    public float minSpreadAngle;
    public float maxSpreadAngle;
    public float totalAmmo;
    public float ammoDrain;
    public Sprite uiIcon;
    public enum FireType
    {
        Auto,
        Semi,
        Bolt,
    }

    public FireType myFireType;
    public float reloadSpeed;
}
