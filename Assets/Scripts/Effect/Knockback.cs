using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Knockback", menuName = "Scriptable Objects/Effects/Knockback", order = 3)]
public class Knockback : Effect
{
    public float forceMultiplier;
    [Range(0f, 2f)] public float releaseTime;
    private Rigidbody2D _targetRb;
    private Vector2 _direction;
    
    public override void Trigger(EffectArgs args)
    {
        if (!args.Target.TryGetComponent<CharacterManager>(out var cm)) return;
        _targetRb = cm.GetComponent<Rigidbody2D>();
        _direction = (args.EffectPoint - args.SourcePoint).normalized;
            
        ActiveRunRoutine = RunRoutine(cm);
        Game.Instance.StartCoroutine(ActiveRunRoutine);
    }

    protected override IEnumerator RunRoutine(CharacterManager target)
    {
        _targetRb.AddForce(_direction * forceMultiplier, ForceMode2D.Impulse);
        // need some way to pause targets movement during knockback
        // could add method to character manager that uses a coroutine to lock movement for the same time as knockback
        yield return new WaitForSeconds(releaseTime);
        
        if (_targetRb != null) // null check in case character died after being knocked back
        {
            _targetRb.velocity = Vector2.zero;
        }
    }
}
