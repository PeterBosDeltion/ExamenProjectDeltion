using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipHolder : MonoBehaviour
{
    //Class to hold all audioclips to not make the inspector a living hell to navigate
    [Header("Voice Lines")]
    public AudioClip voiceAffirmative;
    public AudioClip voiceNegative;
    public AudioClip voiceGogogo;
    public AudioClip voiceHurt;
    public AudioClip voiceReloading;
    public AudioClip voiceNeedReload;
    public AudioClip voiceEmptyMag;
    public AudioClip voiceOutOfAmmo;
    public AudioClip voiceSwappingMag;
    public AudioClip voiceLowAmmo;
    public AudioClip voiceLaugh;
    public AudioClip voiceManiacalLaugh;
    public AudioClip voiceMissionComplete;
    public AudioClip voiceObjectiveLocated;
    public AudioClip voiceObjectiveDestroyed;
    public AudioClip voiceObjectiveComplete;
    public AudioClip voiceNeedHealing;
    public AudioClip voiceNeedHelp;
    public AudioClip voiceUltCharged;
    public AudioClip voiceUltReady;
    public AudioClip voiceTauntOne;
    public AudioClip voiceTauntTwo;
    public AudioClip voiceDeployableDestroyed;

    [Space(10)]
    [Header("Explosions")]
    public AudioClip smallExplosion;
    public AudioClip largeExplosion;
    public AudioClip hugeExplosion;

    [Space(10)]
    [Header("Enemies")]
    public AudioClip enemyWalk;
    public AudioClip enemyAttack;
    public AudioClip enemyDie;

    [Space(10)]
    [Header("Weapon effects")]
    public AudioClip ballisticShoot;
    public AudioClip laserShoot;
    public AudioClip reloadThreeSeconds;
    public AudioClip emptyClick;

    [Space(10)]
    [Header("Ability effects")]
    public AudioClip abilityUsedGeneric;
    public AudioClip fireAbilityUsed;
    public AudioClip droneBeep;
    public AudioClip turretShoot;
    public AudioClip healBeam;

    [Space(10)]
    [Header("Menu effects")]
    public AudioClip menuButtonHover;
    public AudioClip menuButtonClicked;

    [Space(10)]
    [Header("Music")]
    public AudioClip musicMainTheme;
    public AudioClip musicMenuTheme;
    public AudioClip musicCombatTheme;
}
