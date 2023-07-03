using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

[CreateAssetMenu(fileName = "AOE", menuName = "Scriptable Objects/Effects/AOE")]
public class AOE : Effect
{
    public float radius;
    public List<Effect> areaEffects;
    public LayerMask hitDetectionMask;
    public bool losRequired;
    public LayerMask losInterruptionMask;
    
    public override void Trigger(EffectArgs args)
    {
        Debug.DrawLine(args.EffectPoint, args.EffectPoint + new UnityEngine.Vector2(radius, 0f), Color.cyan, 3f);
        Debug.DrawLine(args.EffectPoint, args.EffectPoint + new UnityEngine.Vector2(-radius, 0f), Color.cyan, 3f);
        Debug.DrawLine(args.EffectPoint, args.EffectPoint + new UnityEngine.Vector2(0f, radius), Color.cyan, 3f);
        Debug.DrawLine(args.EffectPoint, args.EffectPoint + new UnityEngine.Vector2(0f, -radius), Color.cyan, 3f);
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(args.EffectPoint, radius, hitDetectionMask);
        
        // trigger area particles
        
        if (hitColliders.Length == 0) return;

        foreach (var c in hitColliders)
        {
            if (c.TryGetComponent<CharacterManager>(out var cm))
            {
                var cmTr = cm.transform;
                var hitArgs = new EffectArgs()
                {
                    Source = args.Source,
                    Target = cmTr,
                    SourcePoint = args.EffectPoint,
                    EffectPoint = cmTr.position
                };
                
                TriggerEffectList(areaEffects, hitArgs);
            }
        }
    }
}
