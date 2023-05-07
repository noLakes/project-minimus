using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class CheckHasBuildOrder: Node
{
    StructureManager manager;
    Timer buildTimer;

    public CheckHasBuildOrder(StructureManager manager, Timer buildTimer)
    {
        this.manager = manager;
        this.buildTimer = buildTimer;
    }

    public override NodeState Evaluate()
    {
        object buildQueue = root.GetData("buildQueue");

        if (buildQueue == null)
        {
            root.ClearData("buildQueue");
            state = NodeState.FAILURE;
            return state;
        }

        List<AbilityManager> buildQueueList = (List<AbilityManager>)buildQueue;

        if(buildQueueList.Count == 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        object activeBuildOrder = root.GetData("activeBuildOrder");

        if (activeBuildOrder == null)
        {
            root.SetData("activeBuildOrder", buildQueueList[0]);
            buildTimer.SetTimer(buildQueueList[0].ability.castTime, manager.UpdateProgressBar);
        }

        state = NodeState.SUCCESS;
        return state;
    }
}