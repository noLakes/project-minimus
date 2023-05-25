using UnityEngine;

using BehaviorTree;

public class CheckHasTarget: Node
{
    public override NodeState Evaluate()
    {
        object currentTarget = GetData("currentTarget");
        if (currentTarget == null)
        {
            ClearData("followDestination");
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }

        // (in case the target object is gone - for example it died
        // and we haven't cleared it from the data yet)
        if (!((Transform) currentTarget))
        {
            ClearData("followDestination");
            ClearData("currentTarget");
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return _state;
        }

        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}