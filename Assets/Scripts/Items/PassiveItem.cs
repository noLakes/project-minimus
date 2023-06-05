using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveItem
{
    private PassiveItemData _data;
    
    public PassiveItem(PassiveItemData data)
    {
        _data = data;
    }

    public void ApplyModsToCharacter(Character character)
    {
        var args = new EffectArgs(
            character.Transform, 
            character.Transform, 
            character.Transform.position
            );
        
        Effect.ApplyEffectList(_data.passiveEffects, args);

        foreach (var effect in _data.onHitEffects)
        {
            character.Stats.AddOnHitEffect(effect);
        }
    }

    public void RemoveModsFromCharacter(Character character)
    {
        foreach (var effect in _data.passiveEffects)
        {
            effect.Remove();
        }
        
        foreach (var effect in _data.onHitEffects)
        {
            character.Stats.RemoveOnHitEffect(effect);
        }
    }
    
    public static void CreatePickup(PassiveItemData data, Vector2 location)
    {
        Transform tr = GameObject.Instantiate(data.pickupPrefab, location, Quaternion.identity).transform;
        var collider = tr.AddComponent<CircleCollider2D>();
        collider.radius = 1.5f; // update to reference global pickup range in future
        collider.isTrigger = true;

        var pickup = tr.AddComponent<PassiveItemPickup>();
        pickup.Initialize(data);
    }
    
    public PassiveItemData Data => _data;
}
