using UnityEngine;

using BehaviorTree;

public class TaskAttack : Node
{
    CharacterManager manager;

    public TaskAttack(CharacterManager manager) : base()
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = root.GetData("currentTarget");
        var targetTransform = (Transform)currentTarget;
        manager.Attack(targetTransform.position);
        state = NodeState.SUCCESS;
        return state;
    }
}