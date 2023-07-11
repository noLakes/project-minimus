using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct EffectArgs
{
    public Transform Target;
    public Transform Source;
    public Vector2 SourcePoint;
    public Vector2 EffectPoint;
    
    // could reference all sorts of optional parameters

    public EffectArgs(Transform source, Transform target, Vector2 effectPoint)
    {
        Source = source;
        SourcePoint = Source.transform.position;
        Target = target;
        EffectPoint = effectPoint;
    }
}

public enum EffectType
{
    TargetInstant,
    TargetPersistent,
    WorldSpace
}

public enum EffectProperty
{
    Buff,
    Debuff,
    Damage,
    Heal
}

public enum EffectCondition
{
    Exclusive,
    TargetHealthy,
    SourceHealthy,
    TargetDamaged,
    SourceDamaged,
    TargetDying,
    SourceDying
}

public abstract class Effect : ScriptableObject
{
    protected IEnumerator ActiveRunRoutine;
    public EffectType type;
    public List<EffectCondition> conditions;
    public GameObject effectParticles;

    public abstract void Trigger(EffectArgs args);

    public virtual void Remove()
    {
        // do nothing by default
    }

    // handle logic for running effect over time
    protected virtual IEnumerator RunRoutine(CharacterManager target)
    {
        // do nothing by default
        yield return null;
    }

    public static void TriggerEffectList(List<Effect> effects, EffectArgs args)
    {
        foreach (var effect in effects)
        {
            effect.Trigger(args);
        }
    }

    protected bool ConditionsAreValid(EffectArgs args)
    {
        //return conditions.All(condition => ValidateCondition(condition, args, this));
        foreach (var cnd in conditions)
        {
            if (!ValidateCondition(cnd, args, this))
            {
                Debug.Log("Condition failed: " + cnd.ToString());
                return false;
            }
        }
        return true;
    }

    public static bool ValidateCondition(EffectCondition condition, EffectArgs args, Effect effect)
    {
        bool result = false;

        switch (condition)
        {
            case EffectCondition.Exclusive:
            {
                if (args.Target.transform.TryGetComponent<CharacterManager>(out var cm))
                {
                    result = !cm.AffectedBy(effect);
                }
                break;
            }
        }
        
        return result;
    }
}
