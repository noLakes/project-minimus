using UnityEngine;

using BehaviorTree;

public class TaskWander : Node
{
    private AICharacterManager _aiCharacterManager;
    private float _fovRadius;

    public TaskWander(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
        _fovRadius = aiCharacterManager.Character.Stats.fovRadius;
    }

    public override NodeState Evaluate()
    {
        var valid = false;

        var wanderPoint = Vector2.zero;

        while (!valid)
        {
            wanderPoint = GenerateWanderPoint();

            if (_aiCharacterManager.ValidPathTo(wanderPoint)) valid = true;
        }

        //Debug.Log("Wandering to: " + wanderPoint);

        Parent.Parent.SetData("destinationPoint", (object)wanderPoint);

        _state = NodeState.SUCCESS;
        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }

    private Vector2 GenerateWanderPoint()
    {
        var distance = Random.Range((_fovRadius * 0.2f), _fovRadius);
        var direction = Random.insideUnitCircle;
        //Debug.Log("Wander distance: " + distance + " / In direction: " + direction);
        return (Vector2)_aiCharacterManager.transform.position + (direction * distance);
    }
}