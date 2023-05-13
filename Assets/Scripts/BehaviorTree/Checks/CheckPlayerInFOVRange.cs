using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInFOVRange : Node
{
    // this class may need to be changed to handle scanning for multiple enemies in future
    
    private AICharacterManager _aiCharacterManager;
    private float _fovRadius;
    private Transform _transform;
    
    public CheckPlayerInFOVRange(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
        _fovRadius = _aiCharacterManager.Character.Stats.fovRadius;
        _transform = _aiCharacterManager.transform;
    }

    public override NodeState Evaluate()
    {
        var playerDistance = Vector2.Distance(
            _transform.position,
            Game.Instance.PlayerCharacter.transform.position
            );

        if (playerDistance <= _fovRadius)
        {
            root.SetData("currentTarget", Game.Instance.PlayerCharacter.transform);
            state = NodeState.SUCCESS;
            root.Wake();
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}