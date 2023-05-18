using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorTree;

public class CheckCanSeePlayer : Node
{
    private AICharacterManager _aiCharacterManager;
    private float _fovRadius;
    private Transform _transform;
    private int _layerMask = ~(1 << 7 | 1 << 9);
    
    public CheckCanSeePlayer(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
        _fovRadius = _aiCharacterManager.Character.Stats.fovRadius;
        _transform = _aiCharacterManager.transform;
    }

    public override NodeState Evaluate()
    {
        Transform player = Game.Instance.PlayerCharacter.transform;
        var playerDistance = Vector2.Distance(
            _transform.position,
            player.position
        );

        if (playerDistance <= _fovRadius && Utility.HasLineOfSight(_transform, player, _fovRadius, _layerMask))
        {
            Parent.Parent.SetData("currentTarget", Game.Instance.PlayerCharacter.transform);
            _state = NodeState.SUCCESS;
            return _state;
        }
        
        Debug.Log("Cannot see Player");
        _state = NodeState.FAILURE;
        return _state;
    }
}
