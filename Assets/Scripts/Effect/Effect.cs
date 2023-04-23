using System.Collections;
using UnityEngine;

public struct EffectArgs
{
    public Transform Target;
    public Transform Source;
    public Vector2 SourcePoint;
    public Vector2 EffectPoint;

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
    // store reference to run routine if effects is applied over time
    protected IEnumerator activeRunRoutine;
    public GameObject effectParticles;

    // apply to target
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
