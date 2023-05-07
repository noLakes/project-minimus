using UnityEngine;
using BehaviorTree;

public class TaskWake : Node
{
    CharacterManager manager;

    public TaskWake(CharacterManager manager)
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        root.Wake();
        state = NodeState.SUCCESS;
        return state;
    }
}