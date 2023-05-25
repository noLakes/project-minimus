using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckWasAttacked : Node
{
    public override NodeState Evaluate()
    {
        object attackOrigin = GetData("attackedFromOrigin");
        if (attackOrigin == null)
        {
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }
        
        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}
