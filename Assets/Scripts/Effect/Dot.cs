using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DOT", menuName = "Scriptable Objects/Effects/DOT", order = 1)]
public class Dot : Effect
{
    public float duration;
    public float tickInterval;
    public int damage;
    private CharacterManager _target;

    public override void Trigger(EffectArgs args)
    {
        if (!args.Target.TryGetComponent<CharacterManager>(out var cm)) return;
        _target = cm;
        _target.AddEffect(this);
        ActiveRunRoutine = RunRoutine(cm);
        Game.Instance.StartCoroutine(ActiveRunRoutine);
    }

    public override void Remove()
    {
        _target.RemoveEffect(this);
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
            //Debug.Log("DoT did " + damage + " dmg");
            yield return new WaitForSeconds(tickInterval);
            timeElapsed += tickInterval;
        }
        
        _target.RemoveEffect(this);
    }
}
