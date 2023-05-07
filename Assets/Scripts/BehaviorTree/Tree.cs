using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        protected Node root = null;
        private float tickTimer;
        public float tickRate;

        public bool awake { get => root.awake; }

        protected void Start()
        {
            root = SetupTree();
            root.PassRootReferenceDown(root);
            tickTimer = 0f;

            // add random jitter to prevent all units instantiated at same time from ticking at same time
            tickRate += Random.Range(-0.05f, 0.05f);
        }

        private void Update()
        {
            if (Game.Instance.GameIsPaused) return;
            
            if (root.dirty)
            {
                RunTree();
            }
            else
            {
                tickTimer += Time.deltaTime;
                if (tickTimer >= tickRate)
                {
                    RunTree();
                    tickTimer = 0f;
                }
            }
        }

        protected void RunTree()
        {
            root.SetDirty(false);
            root.Evaluate();
        }

        protected abstract Node SetupTree();

        public void SetData(string key, object value)
        {
            root.SetData(key, value);
            Wake();
        }

        public void SetDataNextFrame(string key, object value)
        {
            StartCoroutine(NextFramePushDataRoutine(key, value));
        }

        private IEnumerator NextFramePushDataRoutine(string key, object value)
        {
            yield return null;
            SetData(key, value);
            Wake();
        }

        public object GetData(string key)
        {
            return root.GetData(key);
        }

        public bool ClearData(string key)
        {
            return root.ClearData(key);
        }

        public void ClearAllData()
        {
            root.ClearAllData();
        }
        
        public void RunTreeNow()
        {
            tickTimer = 0f;
            RunTree();
        }

        public void Wake() => root.Wake();
        public void Sleep() => root.Sleep();
        public void SetDirty(bool value) => root.SetDirty(value);
    }
}
