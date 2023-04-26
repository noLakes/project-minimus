using System.Collections;
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

public abstract class Effect : ScriptableObject
{
    protected IEnumerator ActiveRunRoutine;
    public GameObject effectParticles;
    
    public abstract void Apply(EffectArgs args);

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
}
