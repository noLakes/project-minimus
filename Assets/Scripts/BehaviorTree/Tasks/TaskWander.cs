using UnityEngine;

using BehaviorTree;

public class TaskWander : Node
{
    AIController _aiController;
    float fovRadius;

    public TaskWander(AIController aiController) : base()
    {
        _aiController = aiController;
        fovRadius = aiController.Character.Stats.fovRadius;
    }

    public override NodeState Evaluate()
    {
        var valid = false;

        Vector2 movePoint = Vector2.zero;

        while (!valid)
        {
            var distance = Random.Range((fovRadius * 0.2f), fovRadius);
            var direction = Random.insideUnitCircle * distance;
            movePoint = (Vector2)_aiController.transform.position + new Vector2(direction.x, direction.y);

            if (_aiController.ValidPathTo(movePoint)) valid = true;
        }

        Debug.Log("Wandering to: " + movePoint);

        root.SetData("destinationPoint", (object)movePoint);

        state = NodeState.SUCCESS;
        return state;
    }
}