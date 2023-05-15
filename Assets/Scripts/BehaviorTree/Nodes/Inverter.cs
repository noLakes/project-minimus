using System.Collections.Generic;

namespace BehaviorTree
{
    public class Inverter : Node
    {
        public Inverter() : base() { }
        public Inverter(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            if (!hasChildren) return NodeState.FAILURE;
            switch (Children[0].Evaluate())
            {
                case NodeState.FAILURE:
                    _state = NodeState.SUCCESS;
                    return State;
                case NodeState.SUCCESS:
                    _state = NodeState.FAILURE;
                    return State;
                case NodeState.RUNNING:
                    _state = NodeState.RUNNING;
                    return State;
                default:
                    _state = NodeState.FAILURE;
                    return State;
            }
        }
    }
}