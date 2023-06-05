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
    [Range(0f, 100f)]public float speed;
    public float rangedDamageModifier;
    public float meleeDamageModifier;
    public List<Effect> onHitEffects;

    public void AddOnHitEffect(Effect e)
    {
        onHitEffects.Add(e);
    }

    public bool RemoveOnHitEffect(Effect e)
    {
        if (onHitEffects.Contains(e))
        {
            return onHitEffects.Remove(e);
        }

        return false;
    }
}