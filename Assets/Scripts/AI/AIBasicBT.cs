using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class AIBT : Tree
{
    private AICharacterManager _aiCharacterManager;
    
    private void Awake()
    {
        _aiCharacterManager = GetComponent<AICharacterManager>();
    }

    protected override Node SetupTree()
    {
        Node root;

        root = new Selector(new List<Node>());

        return root;
    }
}
