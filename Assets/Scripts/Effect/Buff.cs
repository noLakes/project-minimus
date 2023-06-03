using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Effect
{
    public float duration;
    private CharacterManager _character;
    private EffectArgs _initialArgs;
    public List<Effect> targetEffects;

    public override void Apply(EffectArgs args)
    {
        if (!args.Target.TryGetComponent<CharacterManager>(out _character)) return;
        _initialArgs = args;
        ApplyEffects();
        ActiveRunRoutine = RunRoutine(_character);
        Game.Instance.StartCoroutine(ActiveRunRoutine);
    }

    public override void Remove()
    {
        if (ActiveRunRoutine == null) return;
        Game.Instance.StopCoroutine(ActiveRunRoutine);
        ActiveRunRoutine = null;
        
        RemoveAppliedEffects();
    }

    protected override IEnumerator RunRoutine(CharacterManager target)
    {
        yield return new WaitForSeconds(duration);
        if (_character != null) RemoveAppliedEffects();
    }

    protected virtual void ApplyEffects()
    {
        foreach (var effect in targetEffects)
        {
            effect.Apply(_initialArgs);
        }
    }

    protected virtual void RemoveAppliedEffects()
    {
        foreach (var effect in targetEffects)
        {
            effect.Remove();
        }
    }
}
