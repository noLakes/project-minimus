using UnityEngine;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Chance : Node
    {
        private float chance;

        public Chance(float chance) : base()
        {
            this.chance = chance;
        }
        public Chance(float chance, List<Node> children)
            : base(children)
        {
            this.chance = chance;
        }

        public override NodeState Evaluate()
        {
            if (!hasChildren) return NodeState.FAILURE;

            float roll = Random.Range(0f, 1f);

            if(roll <= chance)
            {
                //Debug.Log("Chance node passed with: " + roll + "/" + chance);
                state = children[0].Evaluate();
            }
            else
            {
                state = NodeState.FAILURE;
            }

            return state;
        }
    }
}