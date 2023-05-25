using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskInvestigateAttackOrigin : Node
{
    private AICharacterManager _aiCM;
    private Transform _transform;

    public TaskInvestigateAttackOrigin(AICharacterManager aiCharacterManager)
    {
        _aiCM = aiCharacterManager;
        _transform = _aiCM.transform;
    }
    
    public override NodeState Evaluate()
    {
        object attackOrigin = GetData("attackedFromOrigin");
        
        if (attackOrigin == null)
        {
            _state = NodeState.FAILURE;
            return _state;
        }

        Vector2 position = _transform.position;
        Vector2 attackOriginPoint = (Vector2)attackOrigin;
        Vector2 investigateDir = (attackOriginPoint - position).normalized;
        Vector2 investigatePoint = position + (investigateDir * _aiCM.Character.Stats.fovRadius);
        
        if (!_aiCM.ValidPathTo(investigatePoint))
        {
            investigatePoint = Utility.GetClosePositionWithRadius(investigatePoint, 5f);

            if (investigatePoint == Vector2.zero || !_aiCM.ValidPathTo(investigatePoint)) // no valid path to investigate
            {
                Debug.Log(_aiCM.Character.Code + " Cannot investigate attack origin");
                ClearData("attackedFromOrigin"); // clears investigate point to prevent re-checking same attack instance
                _state = NodeState.FAILURE;
                return _state;
            }
        }
        
        Debug.Log("investigating attack origin direction");
        Debug.DrawLine(position, investigatePoint, Color.green, 5f);
        Parent.Parent.SetData("destinationPoint", investigatePoint);
        ClearData("attackedFromOrigin"); // clears investigate point to prevent re-checking same attack instance
        _state = NodeState.SUCCESS;
        return _state;
    }
}
