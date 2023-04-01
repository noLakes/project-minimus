using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "Scriptable Objects/Effects/Damage", order = 2)]
public class Damage : Effect
{
    public int amount;

    public override void Apply(GameObject target)
    {
        if(target.TryGetComponent<CharacterManager>(out CharacterManager cm))
        {
            cm.Damage(amount);
        }
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator RunRoutine(CharacterManager target)
    {
        throw new System.NotImplementedException();
    }
}
