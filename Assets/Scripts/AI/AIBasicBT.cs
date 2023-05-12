using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class AIBT : Tree
{
    private AIController _aiController;
    
    private void Awake()
    {
        _aiController = GetComponent<AIController>();
    }

    protected override Node SetupTree()
    {
        Node root;

        root = new Selector(new List<Node>());

        return root;
    }
}
