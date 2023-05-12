using UnityEngine;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Timer : Node
    {
        private float delay;
        private float time;
        public bool running { get => state == NodeState.RUNNING; }

        public delegate void TickEnded();
        public delegate void Tick(float taskTime, float timeRemaining);
        public event TickEnded onTickEnded;
        public event Tick onTick;

        public Timer(float delay, Tick onTick = null, TickEnded onTickEnded = null) : base()
        {
            this.delay = delay;
            time = delay;
            this.onTick = onTick;
            this.onTickEnded = onTickEnded;
        }
        public Timer(float delay, List<Node> children, Tick onTick = null, TickEnded onTickEnded = null)
            : base(children)
        {
            this.delay = delay;
            time = delay;
            this.onTick = onTick;
            this.onTickEnded = onTickEnded;
        }

        public override NodeState Evaluate()
        {
            if (!hasChildren) return NodeState.FAILURE;
            if (time <= 0)
            {
                //Debug.Log("Timer complete, triggering ability.");
                time = delay;
                state = children[0].Evaluate();
                if (onTickEnded != null)
                    onTickEnded();
                if (onTick != null)
                    onTick(delay, time);
                //root.liveNodes.Remove(this);
                state = NodeState.SUCCESS;
            }
            else
            {
                //if(!root.liveNodes.Contains(this)) root.liveNodes.Add(this);

                time -= Time.deltaTime;
                if (onTick != null)
                    onTick(delay, time);
                state = NodeState.RUNNING;
            }
            root.SetDirty(true);
            return state;
        }

        public void SetTimer(float delay, Tick onTick = null, TickEnded onTickEnded = null)
        {
            //Debug.Log("Timer updated with delay of: " + delay);
            if (running) state = NodeState.FAILURE;
            this.delay = delay;
            this.time = delay;
            this.onTick = onTick;
            this.onTickEnded = onTickEnded;
            
            //if(root.liveNodes.Contains(this)) root.liveNodes.Remove(this);
        }
    }
}