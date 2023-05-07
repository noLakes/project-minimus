using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskSucceed : Node
{
    public override NodeState Evaluate()
    {
        state = NodeState.SUCCESS;
        return state;
    }
}