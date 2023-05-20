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
            new CheckCanSeeTarget(_aiCharacterManager),
            new CheckTargetInAttackRange(_aiCharacterManager),
            new TaskStopMoving(_aiCharacterManager),
            new TaskStopFollowing(),
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
            new Selector(new List<Node>
            {
                executeAttackNode,
                pursueTargetNode
            })
        });

        var lookForTargetNode = new Sequence(new List<Node>
        {
            new CheckCanSeePlayer(_aiCharacterManager),
            new TaskStopMoving(_aiCharacterManager)
        });

        var movementNode = new Sequence(new List<Node>
        {
            new CheckHasDestination(),
            new TaskMoveToDestination(_aiCharacterManager)
        });

        var wanderNode = new Timer(1f, new List<Node>
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
