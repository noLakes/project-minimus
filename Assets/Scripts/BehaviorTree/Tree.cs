using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        public bool printCallStack;
        
        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (Game.Instance.GameIsPaused) return;
            RunTree();
        }

        private void RunTree()
        {
            _root.Evaluate();
            if (printCallStack) Debug.Log(GetData("callStack"));
            _root.ClearData("callStack");
        }

        protected abstract Node SetupTree();

        // for feeding data in to root node data context
        public void SetData(string key, object value)
        {
            _root.SetData(key, value);
        }

        public object GetData(string key) => _root.GetData(key);
    }
}
