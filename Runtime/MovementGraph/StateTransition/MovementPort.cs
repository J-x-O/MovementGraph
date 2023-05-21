using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Movement.States;
using Gameplay.Movement.States;
using Movement.States;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class MovementPort {

        public IReadOnlyList<Transition> Transitions => _transitions;
        [SerializeField] private List<Transition> _transitions = new List<Transition>();

        [field:SerializeReference] public State State { get; private set; }
        
        public MovementState FindFirstValidTransition() {
            foreach (Transition transition in _transitions) {
                MovementPort compare = transition.NotMe(this);
                MovementState resolve = compare.ResolveActivation();
                if (resolve != null) return resolve;
            }
            return null;
        }

        private MovementState ResolveActivation() => State.ResolveActivation(this);

        public void ConnectTo(MovementPort other) {
            if (other == null) return;
            if (HasDirectTransition(other)) return;
            
            Transition transition = new Transition(this, other);
            _transitions.Add(transition);
            other._transitions.Add(transition);
        }

        /// <summary> Checks if there is a valid transition from this state to the provided target state </summary>
        /// <remarks> This will only check direct connections </remarks>
        /// <param name="target"> the target state we want to check the transition to </param>
        /// <returns> if there is a transition </returns>
        public bool HasTransition(State target) => HasDirectTransition(target, 0);

        private bool HasDirectTransition(State target, int recursionDepth) {
            const int recursionLimit = 10;
            if (recursionDepth >= recursionLimit) {
                string name = State is NamedState casted ? casted.Identifier : "Unnamed";
                Debug.LogWarning($"Hit Recursion Limit for TransitionSearch, starting at Node {name}", State.GameObject);
                return false;
            }
            
            foreach (Transition transition in Transitions) {
                MovementPort compare = transition.NotMe(this);
                if (compare.State == target) return true;
                if (compare.State is not IFastForward redirect) continue;
                
                MovementPort continuePort = redirect.GetNextPort(compare);
                if(continuePort == null) continue;
                if (continuePort.HasDirectTransition(target, recursionDepth + 1)) return true;
            }
            
            return false;
        }

        public bool HasDirectTransition(MovementPort port) {
            foreach (Transition transition in Transitions) {
                MovementPort compare = transition.NotMe(this);
                if (compare == port) return true;
            }
            return false;
        }

        /// <summary> Check if there is a valid transition from the currently active state of the system </summary>
        /// <param name="useAnyState"> if true then the check will include if there is a transition from the any state block </param>
        public bool HasActiveTransition(bool useAnyState) {
            if (HasTransition(State.Layer.CurrentState)) return true;
            return useAnyState 
                   && State.Layer.TryGetState("Any State", out NamedState anyState)
                   && HasTransition(anyState);
        }
    }
}