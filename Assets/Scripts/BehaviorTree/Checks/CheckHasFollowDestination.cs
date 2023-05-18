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
            Debug.Log("no follow");
            _state = NodeState.FAILURE;
            return _state;
        }

        Vector2 followPoint = (Vector2)followDestination;
        Transform target = (Transform)GetData("currentTarget");

        if(Vector2.Distance(followPoint, target.position) >= _aiCharacterManager.CurrentWeapon.Stats.Range)
        {
            Debug.Log("follow out of ideal range");
            _state = NodeState.FAILURE;
            return _state;
        }

        _state = NodeState.SUCCESS;
        return _state;
    }
}