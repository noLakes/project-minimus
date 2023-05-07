using BehaviorTree;

public class CheckUnitIsMine: Node
{
    private bool unitIsMine;

    public CheckUnitIsMine(UnitManager manager) : base()
    {
        unitIsMine = manager.Unit.owner == Game.Instance.humanPlayerID;
    }

    public override NodeState Evaluate()
    {
        state = unitIsMine ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}