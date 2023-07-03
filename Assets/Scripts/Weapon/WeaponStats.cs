using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct WeaponStats
{
    public int damage;
    public float attackRate;
    public int magazineSize;
    public float reloadTime;
    public float range;
    public LayerMask hitDetectionMask;
    public List<Effect> onHitEffects;
    public List<Effect> onAttackEffects; // triggered when user attacks
    public List<Effect> onProjectileDestructionEffects;
}
