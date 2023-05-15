using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskFail : Node
{
    public override NodeState Evaluate()
    {
        _state = NodeState.FAILURE;
        return _state;
    }
}