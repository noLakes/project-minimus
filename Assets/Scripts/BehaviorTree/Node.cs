using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        public NodeState state { get; protected set; }

        public Node parent;
        public Node root;
        // awake = only essential nodes are run
        public bool awake { get; private set; }
        // dirty = needs to be run
        public bool dirty { get; private set; }

        protected List<Node> children = new List<Node>();
        private Dictionary<string, object> dataContext =
            new Dictionary<string, object>();


        public Node()
        {
            parent = null;
            //liveNodes = new List<Node>();
            awake = true;
            dirty = false;
        }

        //public List<Node> liveNodes;

        public Node(List<Node> children) : this()
        {
            SetChildren(children);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetChildren(List<Node> children)
        {
            foreach (Node c in children)
                Attach(c);
        }

        public void Attach(Node child)
        {
            children.Add(child);
            child.parent = this;
        }

        public void Detatch(Node child)
        {
            children.Remove(child);
            child.parent = null;
        }

        public object GetData(string key)
        {
            object value = null;
            if (dataContext.TryGetValue(key, out value))
                return value;

            // traverse parents for value
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            // traverse parents for value to clear
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }

            return false;
        }

        public void ClearAllData()
        {
            dataContext.Clear();
        }

        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        public bool hasChildren { get => children.Count > 0; }

        public void PassRootReferenceDown(Node root)
        {
            this.root = root;
            foreach(Node child in children) child.PassRootReferenceDown(root);
        }

        public void Wake() => awake = true;
        public void Sleep() => awake = false;
        public void SetDirty(bool status) => dirty = status;
    }


}



