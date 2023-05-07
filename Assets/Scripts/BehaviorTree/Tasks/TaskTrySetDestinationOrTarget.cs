using UnityEngine;

using BehaviorTree;

public class TaskTrySetDestinationOrTarget : Node
{
    CharacterManager manager;

    private Ray ray;
    private RaycastHit raycastHit;

    public TaskTrySetDestinationOrTarget(CharacterManager manager) : base()
    {
        this.manager = manager;
    }

    public override NodeState Evaluate()
    {
        if (manager.isSelected && Input.GetMouseButtonUp(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(
                ray,
                out raycastHit,
                1000f,
                Game.ALL_UNIT_MASK
            ))
            {
                UnitManager um = raycastHit.collider.GetComponent<UnitManager>();
                if (um != null)
                {
                    root.SetData("currentTarget", raycastHit.transform);
                    root.ClearData("destinationPoint");
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            else if (Physics.Raycast(
                ray,
                out raycastHit,
                1000f,
                Game.TERRAIN_MASK
            ))
            {
                root.ClearData("currentTarget");
                root.SetData("destinationPoint", raycastHit.point);
                state = NodeState.SUCCESS;
                return state;
            }
        }
        state = NodeState.FAILURE;
        return state;
    }
}