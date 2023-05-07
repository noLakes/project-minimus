using UnityEngine;

using BehaviorTree;

public class CheckHasFollowDestination: Node
{
    CharacterManager manager;

    public CheckHasFollowDestination(CharacterManager manager)
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        object followDestination = root.GetData("followDestination");
        
        if (followDestination == null)
        {
            //Debug.Log("no follow");
            state = NodeState.FAILURE;
            return state;
        }

        Vector3 followPoint = (Vector3)followDestination;
        Transform target = (Transform)root.GetData("currentTarget");

        if(Vector3.Distance(followPoint, target.position) > manager.Unit.attackRange / 2)
        {
            //Debug.Log("follow out of 5 range");
            //root.ClearData("followDestination");
            //Debug.Log("Updating follow dest");
            root.SetData("followDestination", target.position);
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}