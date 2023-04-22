using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Movement.States;
using Gameplay.Movement.States;
using Movement.States;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    [Serializable]
    public class Port {

        public IReadOnlyList<Transition> Transitions => _transitions;
        [SerializeField] private List<Transition> _transitions = new List<Transition>();

        public State State;
        
        public MovementState FindFirstValidTransition() {
            return _transitions
                .Select(transition => transition.To.State.ResolveActivation())
                .FirstOrDefault(resolve => resolve != null);
        }

        /// <summary> Checks if there is a valid transition from this state to the provided target state </summary>
        /// <remarks> This will only check direct connections </remarks>
        /// <param name="target"> the target state we want to check the transition to </param>
        /// <returns> if there is a transition </returns>
        public bool HasTransitionTo(State target) => HasDirectTransition(target, false, 0);

        /// <summary> Checks if there is a valid transition from the provided target state to this state </summary>
        /// <remarks> This will only check direct connections </remarks>
        /// <param name="target"> the target state we want to check the transition from </param>
        /// <returns> if there is a transition </returns>
        public bool HasTransitionFrom(State target) => HasDirectTransition(target, false, 0);
        
        private bool HasDirectTransition(State target, bool reverse, int recursionDepth) {
            const int recursionLimit = 10;
            if (recursionDepth >= recursionLimit) {
                string name = State is NamedState casted ? casted.Identifier : "Unnamed";
                Debug.LogWarning($"Hit Recursion Limit for TransitionSearch, starting at Node {name}", State.GameObject);
                return false;
            }
            
            foreach (Transition transition in Transitions) {
                Port compare = reverse ? transition.From : transition.To;
                if (compare.State == target) return true;
                if (compare.State is not IFastForward redirect) continue;
                
                Port continuePort = redirect.GetNextPort(compare);
                if(continuePort == null) continue;
                if (continuePort.HasDirectTransition(target, reverse, recursionDepth + 1)) return true;
            }
            
            return false;
        }

        /// <summary> Check if there is a valid transition from the currently active state of the system </summary>
        /// <param name="useAnyState"> if true then the check will include if there is a transition from the any state block </param>
        public bool HasTransition(bool useAnyState) {
            if (HasTransitionFrom(State.Layer.CurrentState)) return true;
            return useAnyState 
                   && State.Layer.TryGetState("Any State", out NamedState anyState)
                   && HasTransitionFrom(anyState);
        }
    }
}