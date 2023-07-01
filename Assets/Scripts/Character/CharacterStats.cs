using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterStats
{
    public int maxHealth;
    public int maxWeaponCount;
    public float fovRadius;
    [Range(0f, 100f)] public float speed;
    public int rangedDamageModifier;
    public int meleeDamageModifier;
    public List<Effect> onHitEffects;

    public void AddOnHitEffect(Effect e)
    {
        onHitEffects.Add(e);
    }

    public bool RemoveOnHitEffect(Effect e)
    {
        return onHitEffects.Contains(e) && onHitEffects.Remove(e);
    }
}