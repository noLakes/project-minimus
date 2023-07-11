using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOE", menuName = "Scriptable Objects/Effects/AOE")]
public class AOE : Effect
{
    public float radius;
    public List<Effect> areaEffects;
    public LayerMask hitDetectionMask;
    public bool losRequired;
    public LayerMask losMask;
    
    public override void Trigger(EffectArgs args)
    {
        if (!ConditionsAreValid(args)) return;
        
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
                if (losRequired && !HasLOS(args.EffectPoint, cm.transform, cm.GetSize())) continue;
                
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

    private bool HasLOS(Vector2 origin, Transform target, float targetSize)
    {
        List<Vector2> samplePoints = new List<Vector2>();

        for (var i = 0; i < 5; i++)
        {
            samplePoints.Add((Vector2)target.position + (Random.insideUnitCircle * (targetSize / 4)));
        }

        int validLOSCount = 0;

        foreach (var point in samplePoints)
        {
            Vector2 dir = Utility.GetDirection2D(origin, point);
            float dist = Vector2.Distance(origin, point);
            
            RaycastHit2D ray = Physics2D.Raycast(origin, dir, dist, losMask);

            if (ray.collider == null)
            {
                Debug.DrawLine(origin, point, Color.green, 3f);
                validLOSCount++;
            }
            else
            {
                Debug.DrawLine(origin, point, Color.red, 3f);
                Debug.DrawLine(origin, ray.point, Color.yellow, 3f);
            }
        }
        
        return validLOSCount >= 2;
    }
}
