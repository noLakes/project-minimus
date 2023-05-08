using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInFOVRange : Node
{
    // this class may need to be changed to handle scanning for multiple enemies in future
    
    private AIController _aiController;
    private float _fovRadius;
    private Transform _transform;
    
    public CheckPlayerInFOVRange(AIController aiController) : base()
    {
        _aiController = aiController;
        _fovRadius = _aiController.Character.Stats.fovRadius;
        _transform = _aiController.transform;
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