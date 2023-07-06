using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempEffect", menuName = "Scriptable Objects/Effects/TempEffect", order = 4)]
public class TempEffect : Effect
{
    public float duration;
    private CharacterManager _affectedCM;
    private EffectArgs _initialArgs;
    public List<Effect> targetEffects;

    public override void Trigger(EffectArgs args)
    {
        if (!args.Target.TryGetComponent<CharacterManager>(out _affectedCM)) return;
        Debug.Log("Applying Buff for " + duration + "s");
        
        _initialArgs = args;
        _affectedCM.AddEffect(this);
        ApplyEffects();
        ActiveRunRoutine = RunRoutine(_affectedCM);
        Game.Instance.StartCoroutine(ActiveRunRoutine);
    }

    public override void Remove()
    {
        Debug.Log("Removing Buff");
        _affectedCM.RemoveEffect(this);
        if (ActiveRunRoutine == null) return;
        Game.Instance.StopCoroutine(ActiveRunRoutine);
        ActiveRunRoutine = null;
        
        RemoveAppliedEffects();
    }

    protected override IEnumerator RunRoutine(CharacterManager target)
    {
        yield return new WaitForSeconds(duration);
        if (_affectedCM != null) Remove();
    }

    protected virtual void ApplyEffects()
    {
        foreach (var effect in targetEffects)
        {
            Debug.Log("Applying sub effect: " + effect.name);
            effect.Trigger(_initialArgs);
        }
    }

    protected virtual void RemoveAppliedEffects()
    {
        foreach (var effect in targetEffects)
        {
            Debug.Log("Removing sub effect: " + effect.name);
            effect.Remove();
        }
    }
}
