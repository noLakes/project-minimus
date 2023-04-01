using System.Collections;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    // store reference to run routine if effects is applied over time
    protected IEnumerator activeRunRoutine;
    public GameObject EffectParticles;

    // apply to target
    public abstract void Apply(GameObject target);
    public abstract void Remove();

    // handle logic for running effect over time
    protected abstract IEnumerator RunRoutine(CharacterManager target);
}
