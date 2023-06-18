using UnityEngine;

using BehaviorTree;

public class CheckTargetInAttackRange : Node
{
    private AICharacterManager _aiCharacterManager;

    public CheckTargetInAttackRange(AICharacterManager aiCharacterManager) : base()
    {
        _aiCharacterManager = aiCharacterManager;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = GetData("currentTarget");
        
        if (currentTarget == null)
        {
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return State;
        }

        Transform target = (Transform)currentTarget;

        // (in case the target object is gone - for example it died
        // and we haven't cleared it from the data yet)
        if (!target)
        {
            //Debug.Log("CHECK ENEMY RANGE FAILED. TARGET GONE");
            ClearData("currentTarget");
            _state = NodeState.FAILURE;
            ThrowResultToDebugCallStack(GetType().Name, _state);
            return State;
        }
        
        float range = _aiCharacterManager.CurrentWeapon.ComputedRange + target.GetComponent<CharacterManager>().GetSize() / 2;
        
        // check based on distance from weapon end point if ranged weapon?
        Vector2 checkFromPoint = 
            _aiCharacterManager.CurrentWeapon.Data.type == WeaponType.Ranged ?
            _aiCharacterManager.CurrentWeapon.Transform.Find("weaponEnd").position :
            _aiCharacterManager.transform.position;
        
        bool isInRange = Vector2.Distance(checkFromPoint, target.position) <= range;

        if(isInRange)
        {
            ClearData("followDestination");
            _state = NodeState.SUCCESS;
            //Debug.Log("Attack target IN RANGE");
        }
        else
        {
            _state = NodeState.FAILURE;
            //Debug.Log("Attack target NOT IN RANGE");
        }

        ThrowResultToDebugCallStack(GetType().Name, _state);
        return _state;
    }
}