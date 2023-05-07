using UnityEngine;

using BehaviorTree;

public class CheckTargetIsMine: Node
{
    private int myPlayerId;

    public CheckTargetIsMine(UnitManager manager) : base()
    {
        //myPlayerId = Game.Instance.humanPlayerID;
        myPlayerId = manager.Unit.owner;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = root.GetData("currentTarget");
        UnitManager um = ((Transform)currentTarget).GetComponent<UnitManager>();
        if (um == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        state = um.Unit.owner == myPlayerId ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}