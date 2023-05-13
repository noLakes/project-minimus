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
        object currentTarget = root.GetData("currentTarget");
        var targetTransform = (Transform)currentTarget;
        _aiCharacterManager.TryAttack(targetTransform.position);
        state = NodeState.SUCCESS;
        return state;
    }
}