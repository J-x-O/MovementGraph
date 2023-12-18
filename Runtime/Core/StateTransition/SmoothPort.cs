using System;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.StateTransition {
    
    [Serializable]
    public class SmoothPort {

        [field:NonSerialized] public State State { get; private set; }
        public string Identifier { get; private set; }
        
        public IReadOnlyList<SmoothTransition> Transitions => _transitions;
        [SerializeField] private List<SmoothTransition> _transitions = new List<SmoothTransition>();


        public ExecutableState FindFirstValidTransition() {
            foreach (SmoothTransition transition in _transitions) {
                ExecutableState resolve = transition.Target.ResolveActivation();
                if (resolve != null && resolve.CanBeActivated()) return resolve;
            }
            return null;
        }

        private ExecutableState ResolveActivation() => State.ResolveActivation(this);

        public void ConnectTo(SmoothPort other) {
            if (other == null) return;
            if (HasDirectTransition(other)) return;
            
            _transitions.Add(new SmoothTransition(other));
            other._transitions.Add(new SmoothTransition(this));
        }

        /// <summary> Checks if there is a valid transition from this state to the provided target state </summary>
        /// <remarks> This will only check direct connections </remarks>
        /// <param name="target"> the target state we want to check the transition to </param>
        /// <returns> if there is a transition </returns>
        public bool HasTransition(State target) => FindDirectTransition(target, 0) != null;
        public SmoothPort FindTransition(State target) => FindDirectTransition(target, 0);

        private SmoothPort FindDirectTransition(State target, int recursionDepth) {
            const int recursionLimit = 10;
            if (recursionDepth >= recursionLimit) {
                Debug.LogWarning($"Hit Recursion Limit for TransitionSearch, starting at Node {State.Identifier}", State.GameObject);
                return null;
            }
            
            foreach (SmoothTransition transition in Transitions) {
                if (transition.Target.State == target) return transition.Target;
                if (transition.Target.State is not IFastForward redirect) continue;
                
                SmoothPort continuePort = redirect.GetNextPort(transition.Target);
                if(continuePort == null) continue;
                SmoothPort result = continuePort.FindDirectTransition(target, recursionDepth + 1);
                if (result != null) return result;
            }
            
            return null;
        }

        public SmoothPort FindDirectTransition(SmoothPort port) {
            return Transitions.FirstOrDefault(transition => transition.Target == port)?.Target;
        }
        
        public bool HasDirectTransition(SmoothPort port) {
            return Transitions.Any(transition => transition.Target == port);
        }

        /// <summary> Check if there is a valid transition from the currently active state of the system </summary>
        /// <param name="useAnyState"> if true then the check will include if there is a transition from the any state block </param>
        public bool HasActiveTransition(bool useAnyState) {
            if (HasTransition(State.Parent.StateMachine.CurrentState)) return true;
            return useAnyState 
                   && (State.StateMachine as IStateParent).TryGetState("Any State", out State anyState)
                   && HasTransition(anyState);
        }

        public void OnBeforeSerialize(State state) {
            foreach (SmoothTransition transition in _transitions) {
                transition.OnBeforeSerialize(state);
            }
        }

        public void OnAfterDeserialize(State state, string identifier) {
            State = state;
            
            Identifier = identifier;
        }

        public void OnLateDeserialize(State state) {
            foreach (SmoothTransition transition in _transitions) {
                transition.OnLateDeserialize(state);
            }
        }
    }
}