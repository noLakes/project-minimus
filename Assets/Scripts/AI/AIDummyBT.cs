using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class AIDummyBT : Tree
{
    private AICharacterManager _aiCharacterManager;
    
    private void Awake()
    {
        _aiCharacterManager = GetComponent<AICharacterManager>();
    }

    protected override Node SetupTree()
    {
        var root = new Selector();
        return root;
    }
}
