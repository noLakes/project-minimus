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
            switch (children[0].Evaluate())
            {
                case NodeState.FAILURE:
                    state = NodeState.SUCCESS;
                    return state;
                case NodeState.SUCCESS:
                    state = NodeState.FAILURE;
                    return state;
                case NodeState.RUNNING:
                    state = NodeState.RUNNING;
                    return state;
                default:
                    state = NodeState.FAILURE;
                    return state;
            }
        }
    }
}