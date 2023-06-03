using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ModSpeed", menuName = "Scriptable Objects/Effects/ModSpeed", order = 5)]
public class ModSpeed : Effect
{
    // NOTE
    // this effect needs to be adjusted to clamp speed to prevent >0 values
    // not implemented at the moment due to anticipated character stat changes
    
    public float modifier;
    private EffectArgs _initialArgs;
    private PlayerMovementController _playerMovementController;
    private NavMeshAgent _navMeshAgent;
    
    public override void Apply(EffectArgs args)
    {
        if (!args.Target.TryGetComponent<CharacterManager>(out var cm)) return;
        _initialArgs = args;

        if (cm.TryGetComponent<PlayerMovementController>(out _playerMovementController))
        {
            // increase speed
            _playerMovementController.ModifySpeed(modifier);
            return;
        }

        if (cm.TryGetComponent<NavMeshAgent>(out _navMeshAgent))
        {
            _navMeshAgent.speed += modifier;
            // increase speed
        }
    }

    public override void Remove()
    {
        if (_initialArgs.Target == null) return;

        if (_playerMovementController != null)
        {
            // remove speed
            _playerMovementController.ModifySpeed(-modifier);
            return;
        }

        if (_navMeshAgent != null)
        {
            _navMeshAgent.speed -= modifier;
            // remove speed
        }
    }
}
