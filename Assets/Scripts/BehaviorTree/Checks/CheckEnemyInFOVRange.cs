using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInFOVRange : Node
{
    UnitManager manager;
    float fovRadius;
    int unitOwner;
    Vector3 pos;
    Transform tr;

    Collider[] nearbyColliders;
    int colliderCount;

    //List<Transform> nearbyEnemies;
    Transform[] nearbyEnemies;
    int enemyCount;

    private LayerMask enemyLayerMask;


    public CheckEnemyInFOVRange(UnitManager manager, int maxNonAllocArraySize = 20) : base()
    {
        this.manager = manager;
        fovRadius = manager.Unit.data.fieldOfView;
        unitOwner = manager.Unit.owner;
        tr = manager.transform;

        nearbyColliders = new Collider[maxNonAllocArraySize];
        colliderCount = 0;

        //nearbyEnemies = new List<Transform>();
        nearbyEnemies = new Transform[maxNonAllocArraySize];
        enemyCount = 0;

        enemyLayerMask = manager.owner == Game.Instance.humanPlayerID ? Game.ENEMY_UNIT_MASK : Game.PLAYER_UNIT_MASK;
    }

    public override NodeState Evaluate()
    {
        pos = tr.position;
        colliderCount = Physics.OverlapSphereNonAlloc(pos, fovRadius, nearbyColliders, enemyLayerMask);
        enemyCount = 0;

        for (int i = 0; i < colliderCount; i++)
        {
            //iterate and check for enemy
            if (!manager.IsFriendly(nearbyColliders[i].gameObject))
            {
                // count and store enemies
                //nearbyEnemies.Add(nearbyColliders[i].transform);
                nearbyEnemies[enemyCount] = nearbyColliders[i].transform;
                enemyCount++;
            }
        }

        if (enemyCount > 0)
        {
            root.SetData(
                "currentTarget",
                nearbyEnemies.Select((tr, index) => new { transform = tr, Index = index })
                .Where(x => x.Index < enemyCount)
                .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
                    .First()
                    .transform
            );

            state = NodeState.SUCCESS;
            root.Wake();
            return state;
        }

        /*
        if (nearbyEnemies.Count > 0)
        {
            root.SetData(
                "currentTarget",
                nearbyEnemies
                    .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
                    .First()
                    .transform
            );

            state = NodeState.SUCCESS;
            root.Wake();
            nearbyEnemies.Clear();
            return state;
        }
        */

        state = NodeState.FAILURE;
        //nearbyEnemies.Clear();
        return state;

        /*
        IEnumerable<Collider> enemiesInRange =
            Physics.OverlapSphere(pos, fovRadius, enemyLayerMask)
            .Where(delegate (Collider c)
            {
                UnitManager um = c.GetComponent<UnitManager>();
                if (um == null || !um.Unit.isActive) return false;
                return um.Unit.owner != unitOwner;
            });

        if (enemiesInRange.Any())
        {
            root.SetData(
                "currentTarget",
                enemiesInRange
                    .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
                    .First()
                    .transform
            );

            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
        */
    }
}