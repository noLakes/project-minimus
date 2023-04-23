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
        if(args.Target.TryGetComponent<CharacterManager>(out CharacterManager cm))
        {
            activeRunRoutine = RunRoutine(cm);
            Game.Instance.StartCoroutine(activeRunRoutine);
        }
    }

    public override void Remove()
    {
        if(activeRunRoutine != null)
        {
            Game.Instance.StopCoroutine(activeRunRoutine);
            activeRunRoutine = null;
        }
    }

    protected override IEnumerator RunRoutine(CharacterManager target)
    {
        float timeElapsed = 0f;

        while(timeElapsed < duration)
        {
            if(target != null) target.Damage(damage);
            yield return new WaitForSeconds(tickInterval);
        }
    }
}
