using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.Attributes;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class MovementPort {

        [field:NonSerialized] public State State { get; private set; }
        public string Identifier { get; private set; }
        
        public IReadOnlyList<Transition> Transitions => _transitions;
        [SerializeField] private List<Transition> _transitions = new List<Transition>();


        public MovementState FindFirstValidTransition() {
            foreach (Transition transition in _transitions) {
                MovementState resolve = transition.Target.ResolveActivation();
                if (resolve != null) return resolve;
            }
            return null;
        }

        private MovementState ResolveActivation() => State.ResolveActivation(this);

        public void ConnectTo(MovementPort other) {
            if (other == null) return;
            if (HasDirectTransition(other)) return;
            
            _transitions.Add(new Transition(other));
            other._transitions.Add(new Transition(this));
        }

        /// <summary> Checks if there is a valid transition from this state to the provided target state </summary>
        /// <remarks> This will only check direct connections </remarks>
        /// <param name="target"> the target state we want to check the transition to </param>
        /// <returns> if there is a transition </returns>
        public bool HasTransition(State target) => HasDirectTransition(target, 0);

        private bool HasDirectTransition(State target, int recursionDepth) {
            const int recursionLimit = 10;
            if (recursionDepth >= recursionLimit) {
                Debug.LogWarning($"Hit Recursion Limit for TransitionSearch, starting at Node {State.Identifier}", State.GameObject);
                return false;
            }
            
            foreach (Transition transition in Transitions) {
                if (transition.Target.State == target) return true;
                if (transition.Target.State is not IFastForward redirect) continue;
                
                MovementPort continuePort = redirect.GetNextPort(transition.Target);
                if(continuePort == null) continue;
                if (continuePort.HasDirectTransition(target, recursionDepth + 1)) return true;
            }
            
            return false;
        }

        public bool HasDirectTransition(MovementPort port) {
            return Transitions.Any(transition => transition.Target == port);
        }

        /// <summary> Check if there is a valid transition from the currently active state of the system </summary>
        /// <param name="useAnyState"> if true then the check will include if there is a transition from the any state block </param>
        public bool HasActiveTransition(bool useAnyState) {
            if (HasTransition(State.Layer.CurrentState)) return true;
            return useAnyState 
                   && State.Layer.TryGetState("Any State", out State anyState)
                   && HasTransition(anyState);
        }

        public void OnBeforeSerialize(State state) {
            foreach (Transition transition in _transitions) {
                transition.OnBeforeSerialize(state);
            }
        }

        public void OnAfterDeserialize(State state, string identifier) {
            State = state;
            Identifier = identifier;
        }

        public void OnLateDeserialize(State state) {
            foreach (Transition transition in _transitions) {
                transition.OnLateDeserialize(state);
            }
        }
    }
}