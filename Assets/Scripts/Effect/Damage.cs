using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "Scriptable Objects/Effects/Damage", order = 2)]
public class Damage : Effect
{
    public int amount;

    public override void Trigger(EffectArgs args)
    {
        if(args.Target.TryGetComponent<CharacterManager>(out var cm))
        {
            cm.Damage(amount);
        }
    }
}
