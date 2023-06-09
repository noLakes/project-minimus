using UnityEngine;

using BehaviorTree;

public class CheckHasFollowDestination: Node
{
    private AICharacterManager _aiCharacterManager;

    public CheckHasFollowDestination(AICharacterManager aiCharacterManager)
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        object followDestination = GetData("followDestination");
        
        if (followDestination == null)
        {
            //Debug.Log("no follow");
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }

        Vector2 followPoint = (Vector2)followDestination;
        Vector2 lastSeenPos = (Vector2)GetData("targetLastSeenPos");

        if(Vector2.Distance(followPoint, lastSeenPos) >= _aiCharacterManager.CurrentWeapon.Stats.range / 4)
        {
            //Debug.Log("follow out of ideal range");
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }

        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}