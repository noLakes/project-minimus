using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item
{
    private readonly ItemData _data;
    private readonly Character _owner;
    
    public Item(ItemData data, Character owner)
    {
        _data = data;
        _owner = owner;
        
        ApplyModsToOwner();
    }

    public bool Use(Vector2 targetPosition)
    {
        if (_data.type == ItemType.Passive || _data.onHitEffects.Count == 0) return false;

        
        
        return true;
    }

    private void ApplyModsToOwner()
    {
        var args = new EffectArgs(
            _owner.Transform, 
            _owner.Transform, 
            _owner.Transform.position
            );
        
        Effect.ApplyEffectList(_data.passiveEffects, args);

        foreach (var effect in _data.onHitEffects)
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
        
        foreach (var effect in _data.onHitEffects)
        {
            _owner.Stats.RemoveOnHitEffect(effect);
        }
    }

    public void Unequip()
    {
        RemoveModsFromOwner();
    }

    public ItemData Data => _data;
}
