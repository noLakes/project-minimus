using UnityEngine;

using BehaviorTree;

public class CheckHasFollowDestination: Node
{
    private AIController _aiController;

    public CheckHasFollowDestination(AIController aiController)
    {
        _aiController = aiController;
    }

    public override NodeState Evaluate()
    {
        object followDestination = root.GetData("followDestination");
        
        if (followDestination == null)
        {
            Debug.Log("no follow");
            state = NodeState.FAILURE;
            return state;
        }

        Vector2 followPoint = (Vector2)followDestination;
        Transform target = (Transform)root.GetData("currentTarget");

        if(Vector2.Distance(followPoint, target.position) > _aiController.CharacterManager.CurrentWeapon.Stats.Range / 2)
        {
            Debug.Log("follow out of ideal range");
            Debug.Log("Updating follow dest");
            root.SetData("followDestination", target.position);
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}