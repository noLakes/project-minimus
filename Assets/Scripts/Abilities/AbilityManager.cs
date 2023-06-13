using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private AbilityData _data;
    private Character _owner;
    private bool _ready;
    private IEnumerator _activeCooldownRoutine;

    public void Initialize(AbilityData data, Character owner)
    {
        _data = data;
        _owner = owner;
        _ready = true;
        _activeCooldownRoutine = null;

    }
    
    private IEnumerator CooldownRoutine()
    {
        _ready = false;
        yield return new WaitForSeconds(_data.cooldown);
        _ready = true;
        _activeCooldownRoutine = null;
    }

    public void Trigger(Vector2 location, Transform target = null)
    {
        if(!_ready) return;
        
        EffectArgs e = new EffectArgs(_owner.Transform, target, location);

        Effect.ApplyEffectList(_data.onCastEffects, e);

        // go on cooldown
        _activeCooldownRoutine = CooldownRoutine();
        StartCoroutine(_activeCooldownRoutine);
    }
}
