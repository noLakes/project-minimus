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
        //Debug.Log("Following");
        Vector2 targetPos = (Vector2)GetData("targetLastSeenPos");

        if (!_aiCharacterManager.TryMove(targetPos))
        {
            targetPos = Utility.GetClosePositionWithRadius(targetPos, 5f);

            if (targetPos == Vector2.zero || !_aiCharacterManager.TryMove(targetPos))
            {
                _state = NodeState.FAILURE;
                return _state;
            }
        }
        
        Parent.Parent.SetData("followDestination", targetPos);
        //Debug.Log("RUNNING FOLLOW");
        _state = NodeState.RUNNING;
        return _state;
    }
}