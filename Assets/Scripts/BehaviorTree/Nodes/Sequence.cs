using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        private bool isRandom;

        public Sequence() : base() { isRandom = false; }
        public Sequence(bool isRandom) : base() { this.isRandom = isRandom; }
        public Sequence(List<Node> children, bool isRandom = false) : base(children)
        {
            this.isRandom = isRandom;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            System.Random r = new System.Random();
            return list.OrderBy(x => r.Next()).ToList();
        }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            if (isRandom)
                children = Shuffle(children);

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}