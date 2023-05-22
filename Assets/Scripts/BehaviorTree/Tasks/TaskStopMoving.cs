using UnityEngine;

using BehaviorTree;

public class TaskStopMoving : Node
{
    private AICharacterManager _aiCharacterManager;

    public TaskStopMoving(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        if(_aiCharacterManager.HasPath) _aiCharacterManager.StopMoving();
        
        Debug.Log("Stop moving");
        ClearData("destinationPoint");
        _state = NodeState.SUCCESS;
        return _state;
    }
}