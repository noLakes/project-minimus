using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DOT", menuName = "Scriptable Objects/Effects/DOT", order = 1)]
public class Dot : Effect
{
    public float duration;
    public float tickInterval;
    public int damage;

    public override void Apply(EffectArgs args)
    {
        if (!args.Target.TryGetComponent<CharacterManager>(out var cm)) return;
        ActiveRunRoutine = RunRoutine(cm);
        Game.Instance.StartCoroutine(ActiveRunRoutine);
    }

    public override void Remove()
    {
        if (ActiveRunRoutine == null) return;
        Game.Instance.StopCoroutine(ActiveRunRoutine);
        ActiveRunRoutine = null;
    }

    protected override IEnumerator RunRoutine(CharacterManager target)
    {
        var timeElapsed = 0f;

        while(timeElapsed < duration)
        {
            if(target != null) target.Damage(damage);
            yield return new WaitForSeconds(tickInterval);
            timeElapsed += tickInterval;
        }
    }
}
