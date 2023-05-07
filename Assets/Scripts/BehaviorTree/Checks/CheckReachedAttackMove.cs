using BehaviorTree;
using UnityEngine;

public class CheckReachedAttackMove : Node
{
    CharacterManager manager;

    public CheckReachedAttackMove(CharacterManager manager)
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        object attackMove = root.GetData("attackMove");

        Vector3 attackPoint = (Vector3)attackMove;

        // check to see if the agent has reached the destination
        float d = Vector3.Distance(manager.transform.position, attackPoint);
        if (d <= manager.navMeshAgent.stoppingDistance)
        {
            root.ClearData("attackMove");
            root.ClearData("destinationPoint");
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}