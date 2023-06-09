using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class TaskSucceed : Node
{
    public override NodeState Evaluate()
    {
        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}