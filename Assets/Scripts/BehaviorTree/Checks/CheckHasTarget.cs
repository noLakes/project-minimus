using UnityEngine;

using BehaviorTree;

public class CheckHasTarget: Node
{
    public override NodeState Evaluate()
    {
        object currentTarget = root.GetData("currentTarget");
        if (currentTarget == null)
        {
            root.ClearData("followDestination");
            state = NodeState.FAILURE;
            return state;
        }

        // (in case the target object is gone - for example it died
        // and we haven't cleared it from the data yet)
        if (!((Transform) currentTarget))
        {
            root.ClearData("followDestination");
            root.ClearData("currentTarget");
            state = NodeState.FAILURE;
            return state;
        }
        
        state = NodeState.SUCCESS;
        return state;
    }
}