using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item
{
    private readonly ItemData _data;
    private readonly Character _owner;
    private AbilityManager _abilityManager;
    
    public Item(ItemData data, Character owner)
    {
        _data = data;
        _owner = owner;
        
        ApplyModsToOwner();
        if(_data.onUseAbility != null) CreateAbilityManager();
    }

    public bool Use(Vector2 targetPosition, Transform target = null)
    {
        if (_data.type == ItemType.Passive || _data.onUseAbility == null) return false;
        
        _abilityManager.Trigger(targetPosition, target);
        
        return true;
    }

    private void ApplyModsToOwner()
    {
        var args = new EffectArgs(
            _owner.Transform, 
            _owner.Transform, 
            _owner.Transform.position
            );
        
        Effect.TriggerEffectList(_data.passiveEffects, args);

        foreach (var effect in _data.conferedOnHitEffects)
        {
            _owner.Stats.AddOnHitEffect(effect);
        }
    }

    private void RemoveModsFromOwner()
    {
        foreach (var effect in _data.passiveEffects)
        {
            effect.Remove();
        }
        
        foreach (var effect in _data.conferedOnHitEffects)
        {
            _owner.Stats.RemoveOnHitEffect(effect);
        }
    }

    public void Unequip()
    {
        RemoveModsFromOwner();
    }

    private AbilityManager CreateAbilityManager()
    {
        var am = _owner.Transform.AddComponent<AbilityManager>();
        am.Initialize(_data.onUseAbility, _owner);
        return am;
    }

    public ItemData Data => _data;
    public AbilityManager AbilityManager => _abilityManager;
}
