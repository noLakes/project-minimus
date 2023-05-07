using UnityEngine;

using BehaviorTree;

public class TaskSetIdle : Node
{
    CharacterManager manager;

    public TaskSetIdle(CharacterManager manager) : base()
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        if (!manager.isIdle)
        {
            manager.ActAsNavObstacle();
            Debug.Log("Set Idle");
        }

        state = NodeState.SUCCESS;
        return state;

        /*
        old
        if(!manager.isIdle && !manager.moving)
        {
            //Debug.Log("Setting " + manager.gameObject.name + " to idle");
            manager.ActAsNavObstacle();
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
        */
    }
}