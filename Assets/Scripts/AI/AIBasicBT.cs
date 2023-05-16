using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class AIBasicBT : Tree
{
    private AICharacterManager _aiCharacterManager;
    
    private void Awake()
    {
        _aiCharacterManager = GetComponent<AICharacterManager>();
    }

    protected override Node SetupTree()
    {
        var executeAttackNode = new Sequence(new List<Node>
        {
            new CheckEnemyInAttackRange(_aiCharacterManager),
            new TaskStopMoving(_aiCharacterManager),
            new TaskAttack(_aiCharacterManager)
        });

        var pursueTargetNode = new Selector(new List<Node>
        {
            new CheckHasFollowDestination(_aiCharacterManager),
            new TaskFollow(_aiCharacterManager)
        });
        
        var mainAttackNode = new Sequence(new List<Node>
        {
            new CheckHasTarget(),
            new CheckCanSeeTarget(_aiCharacterManager),
            new Selector(new List<Node>
            {
                executeAttackNode,
                pursueTargetNode
            })
        });

        var lookForTargetNode = new Selector(new List<Node>
        {
            new CheckCanSeePlayer(_aiCharacterManager)
        });

        var movementNode = new Sequence(new List<Node>
        {
            new CheckHasDestination(),
            new TaskMoveToDestination(_aiCharacterManager)
        });

        var wanderNode = new Timer(5f, new List<Node>
        {
            new TaskWander(_aiCharacterManager)
        });

        var root = new Selector(new List<Node>
        {
            mainAttackNode,
            lookForTargetNode,
            movementNode,
            wanderNode
        });
        
        return root;
    }
}
