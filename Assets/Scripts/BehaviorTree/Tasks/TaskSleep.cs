using UnityEngine;
using BehaviorTree;

public class TaskSleep : Node
{
    CharacterManager manager;

    public TaskSleep(CharacterManager manager)
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        root.Sleep();
        manager.OnSleep();
        state = NodeState.SUCCESS;
        return state;
    }
}