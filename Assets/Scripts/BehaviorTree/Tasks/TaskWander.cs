using UnityEngine;

using BehaviorTree;

public class TaskWander : Node
{
    AICharacterManager _aiCharacterManager;
    float fovRadius;

    public TaskWander(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
        fovRadius = aiCharacterManager.Character.Stats.fovRadius;
    }

    public override NodeState Evaluate()
    {
        var valid = false;

        Vector2 movePoint = Vector2.zero;

        while (!valid)
        {
            var distance = Random.Range((fovRadius * 0.2f), fovRadius);
            var direction = Random.insideUnitCircle * distance;
            movePoint = (Vector2)_aiCharacterManager.transform.position + new Vector2(direction.x, direction.y);

            if (_aiCharacterManager.ValidPathTo(movePoint)) valid = true;
        }

        Debug.Log("Wandering to: " + movePoint);

        root.SetData("destinationPoint", (object)movePoint);

        state = NodeState.SUCCESS;
        return state;
    }
}