using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerLineOfSight : Node
{
    // this class may need to be changed to handle scanning for multiple enemies in future
    
    private AICharacterManager _aiCharacterManager;
    private float _fovRadius;
    private Transform _transform;
    private int layerMask = ~(1 << 7 | 1 << 9);
    
    public CheckPlayerLineOfSight(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
        _fovRadius = _aiCharacterManager.Character.Stats.fovRadius;
        _transform = _aiCharacterManager.transform;
    }

    public override NodeState Evaluate()
    {
        var playerDistance = Vector2.Distance(
            _transform.position,
            Game.Instance.PlayerCharacter.transform.position
            );

        if (playerDistance <= _fovRadius && HasLineOfSight())
        {
            root.SetData("currentTarget", Game.Instance.PlayerCharacter.transform);
            state = NodeState.SUCCESS;
            root.Wake();
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }

    private bool HasLineOfSight()
    {
        var dir = (Game.Instance.PlayerCharacter.transform.position - _transform.position).normalized;
        RaycastHit2D ray = Physics2D.Raycast(_transform.position, dir, _fovRadius, layerMask);

        if (ray.collider == null) return false;
        
        //Debug.Log(ray.collider.gameObject.name);
        Debug.DrawLine(_transform.position, ray.collider.transform.position, Color.yellow, 0.25f);
        
        return ray.collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }
}