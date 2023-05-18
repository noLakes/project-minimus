using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskFollow : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskFollow(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Following");
        Transform currentTarget = (Transform)GetData("currentTarget");

        Vector2 targetPosition = currentTarget.position;

        if (!_aiCharacterManager.TryMove(targetPosition))
        {
            targetPosition = Utility.GetClosePositionWithRadius(currentTarget.position, 5f);

            if (targetPosition == Vector2.zero || !_aiCharacterManager.TryMove(targetPosition))
            {
                _state = NodeState.FAILURE;
                return _state;
            }
        }
        
        Parent.Parent.SetData("followDestination", (object)targetPosition);
        Debug.Log("RUNNING FOLLOW");
        _state = NodeState.RUNNING;
        return _state;
    }
}