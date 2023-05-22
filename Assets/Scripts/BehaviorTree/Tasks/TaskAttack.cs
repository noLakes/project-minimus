using UnityEngine;

using BehaviorTree;

public class TaskAttack : Node
{
    AICharacterManager _aiCharacterManager;

    public TaskAttack(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        //object currentTarget = GetData("currentTarget");
        //var targetTransform = (Transform)currentTarget;
        _state = _aiCharacterManager.CurrentWeapon.CanAttack ? NodeState.SUCCESS : NodeState.RUNNING;
        if(_state == NodeState.SUCCESS) _aiCharacterManager.Attack();
        return _state;
    }
}