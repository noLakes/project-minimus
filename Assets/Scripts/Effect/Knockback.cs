using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Knockback", menuName = "Scriptable Objects/Effects/Knockback", order = 3)]
public class Knockback : Effect
{
    public float forceMultiplier;
    [Range(0f, 2f)] public float runTime;
    private Rigidbody2D _targetRb;
    private Vector2 _direction;
    public override void Apply(EffectArgs args)
    {
        if(args.Target.TryGetComponent<CharacterManager>(out CharacterManager cm))
        {
            _targetRb = cm.GetComponent<Rigidbody2D>();
            _direction = (args.EffectPoint - args.SourcePoint).normalized;
            
            activeRunRoutine = RunRoutine(cm);
            Game.Instance.StartCoroutine(activeRunRoutine);
        }
    }

    protected override IEnumerator RunRoutine(CharacterManager target)
    {
        _targetRb.AddForce(_direction * forceMultiplier, ForceMode2D.Impulse);
        // need some way to pause targets movement during knockback
        // could add method to character manager that uses a coroutine to lock movement for the same time as knockback
        yield return new WaitForSeconds(runTime);
        _targetRb.velocity = Vector2.zero;
        
    }
}