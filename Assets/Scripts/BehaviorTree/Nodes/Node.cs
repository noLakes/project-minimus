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
        protected NodeState _state;
        private Node _parent;

        protected List<Node> Children = new List<Node>();
        private Dictionary<string, object> _dataContext =
            new Dictionary<string, object>();


        public Node()
        {
            _parent = null;
        }
        
        public Node(List<Node> children) : this()
        {
            SetChildren(children);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        private void SetChildren(List<Node> children)
        {
            foreach (Node c in children)
                Attach(c);
        }

        public void Attach(Node child)
        {
            Children.Add(child);
            child._parent = this;
        }

        public void Detatch(Node child)
        {
            Children.Remove(child);
            child._parent = null;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            // traverse parents for value
            Node node = _parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node._parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            // traverse parents for value to clear
            Node node = _parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node._parent;
            }

            return false;
        }

        public void ClearAllData()
        {
            _dataContext.Clear();
        }

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        // used for debugging
        protected void ThrowResultToDebugCallStack(string nodeName, NodeState result)
        {
            if (_parent != null)
            {
                _parent.ThrowResultToDebugCallStack(nodeName, result);
            }
            else
            {
                string resultText = nodeName + ": " + result;
                // append result to callstack string
                object callStackText = GetData("callStack");
                
                if (callStackText == null)
                    SetData("callStack", resultText);
                else
                {
                    SetData("callStack", (string)callStackText + " / " + resultText);
                }
            }
        }

        public bool hasChildren { get => Children.Count > 0; }
        public NodeState State => _state;
        public Node Parent => _parent;
    }
}



