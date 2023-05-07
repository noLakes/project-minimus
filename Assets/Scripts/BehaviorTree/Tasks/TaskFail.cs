using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskFail : Node
{
    public override NodeState Evaluate()
    {
        state = NodeState.FAILURE;
        return state;
    }
}