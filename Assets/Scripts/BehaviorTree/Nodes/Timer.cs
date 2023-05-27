using UnityEngine;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Timer : Node
    {
        private float _delay;
        private float _time;
        public bool running { get => State == NodeState.RUNNING; }

        public delegate void TickEnded();
        public delegate void Tick(float taskTime, float timeRemaining);
        public event TickEnded onTickEnded;
        public event Tick onTick;

        public Timer(float delay, Tick onTick = null, TickEnded onTickEnded = null) : base()
        {
            _delay = delay;
            _time = delay;
            this.onTick = onTick;
            this.onTickEnded = onTickEnded;
        }
        public Timer(float delay, List<Node> children, Tick onTick = null, TickEnded onTickEnded = null)
            : base(children)
        {
            _delay = delay;
            _time = delay;
            this.onTick = onTick;
            this.onTickEnded = onTickEnded;
        }

        public override NodeState Evaluate()
        {
            if (!hasChildren) return NodeState.FAILURE;
            if (_time <= 0)
            {
                //Debug.Log("Timer complete, triggering ability.");
                _time = _delay;
                _state = Children[0].Evaluate();
                onTickEnded?.Invoke();
                onTick?.Invoke(_delay, _time);
                //root.liveNodes.Remove(this);
                _state = NodeState.SUCCESS;
            }
            else
            {
                _time -= Time.deltaTime;
                onTick?.Invoke(_delay, _time);
                _state = NodeState.RUNNING;
            }
            return _state;
        }

        public void SetTimer(float delay, Tick onTick = null, TickEnded onTickEnded = null)
        {
            //Debug.Log("Timer updated with delay of: " + delay);
            if (running) _state = NodeState.FAILURE;
            _delay = delay;
            _time = delay;
            this.onTick = onTick;
            this.onTickEnded = onTickEnded;
        }
    }
}