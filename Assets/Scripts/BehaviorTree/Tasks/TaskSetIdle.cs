using UnityEngine;

using BehaviorTree;

public class TaskSetIdle : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskSetIdle(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        if (!_aiCharacterManager.IsIdle)
        {
            //_aiController.ActAsNavObstacle(); may not be needed in this project
            _aiCharacterManager.SetIdleStatus(true);
            //Debug.Log("Set Idle");
        }

        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}