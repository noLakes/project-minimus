using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        private float _tickTimer;
        public float tickRate;
        
        protected void Start()
        {
            _root = SetupTree();
            _tickTimer = 0f;

            // add random jitter to prevent all units instantiated at same time from ticking at same time
            tickRate += Random.Range(-0.05f, 0.05f);
        }

        private void Update()
        {
            if (Game.Instance.GameIsPaused) return;
            
            _tickTimer += Time.deltaTime;
            if (!(_tickTimer >= tickRate)) return;
            
            RunTree();
            _tickTimer = 0f;
        }

        private void RunTree()
        {
            _root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}
