using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskSucceed : Node
{
    public override NodeState Evaluate()
    {
        _state = NodeState.SUCCESS;
        return _state;
    }
}