using UnityEngine;

using BehaviorTree;

public class TaskWander : Node
{
    CharacterManager manager;
    float fovRadius;

    public TaskWander(CharacterManager manager) : base()
    {
        this.manager = manager;
        fovRadius = manager.Unit.data.fieldOfView;
    }

    public override NodeState Evaluate()
    {
        bool valid = false;

        Vector3 movePoint = Vector3.zero;
        Vector3 direction;
        float distance = 0f;

        while (!valid)
        {
            distance = Random.Range((fovRadius * 0.2f), fovRadius);
            direction = Random.insideUnitCircle * distance;
            movePoint = manager.transform.position + new Vector3(direction.x, 0f, direction.y);

            if (manager.ValidPathTo(movePoint)) valid = true;
        }

        //Debug.Log("Wandering to: " + movePoint);

        root.SetData("destinationPoint", (object)movePoint);

        state = NodeState.SUCCESS;
        return state;
    }
}