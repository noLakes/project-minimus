using UnityEngine;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Chance : Node
    {
        private float _chance;

        public Chance(float chance) : base()
        {
            _chance = chance;
        }
        public Chance(float chance, List<Node> children)
            : base(children)
        {
            _chance = chance;
        }

        public override NodeState Evaluate()
        {
            if (!hasChildren) return NodeState.FAILURE;

            float roll = Random.Range(0f, 1f);

            if(roll <= _chance)
            {
                //Debug.Log("Chance node passed with: " + roll + "/" + chance);
                _state = Children[0].Evaluate();
            }
            else
            {
                _state = NodeState.FAILURE;
            }

            return State;
        }
    }
}