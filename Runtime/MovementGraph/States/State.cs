using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public abstract class State {
                        
        public IReadOnlyList<Transition> Transitions => _transitions;
        [SerializeField] protected List<Transition> _transitions;

#if UNITY_EDITOR
        [HideInInspector] [SerializeField] protected Vector2 _position;
#endif
        
        public MovementSystem System { get; private set; }
        
        protected Transform _transform => System.transform;
        protected GameObject _gameObject => System.gameObject;

        public virtual void Awake(MovementSystem system) {
            System = system;
        }
        
        public abstract bool ValidActivation();
        
        /// <summary> This function decides if the state can be activated or not </summary>
        /// <returns> The resolved movement state for activation, or null if we cant activate </returns>
        public abstract MovementState ResolveActivation();

        
        protected MovementState FindFirstValidTransition() {
            return _transitions
                .Select(transition => transition.Target.ResolveActivation())
                .FirstOrDefault(resolve => resolve != null);
        }
        
        /// <summary> Checks if there is a valid transition from this state to the provided target state </summary>
        /// <remarks> This will only check direct connections </remarks>
        /// <param name="target"> the target state we want to check the transition to </param>
        /// <returns> if there is a transition </returns>
        public bool HasTransitionTo(State target, int recursionDepth = 0) {
            
            const int recursionLimit = 50;
            if (recursionDepth >= recursionLimit) {
                string name = this is NamedState casted ? casted.Identifier : "Unnamed";
                Debug.LogWarning($"Hit Recursion Limit for TransitionSearch, starting at Node {name}", _gameObject);
            }
            
            foreach (Transition transition in Transitions) {
                if (transition.Target == target) return true;
                if (transition.Target is not RedirectState redirect) continue;
                
                if (recursionDepth >= recursionLimit) continue;
                if (redirect.HasTransitionTo(target, recursionDepth + 1)) return true;
            }
            return false;
        }

        /// <summary> Check if there is a valid transition from the currently active state of the system </summary>
        /// <param name="useAnyState"> if true then the check will include if there is a transition from the any state block </param>
        public bool HasTransition(bool useAnyState) {
            if (System.CurrentState.HasTransitionTo(this)) return true;
            return useAnyState 
                   && System.TryGetState("Any State", out NamedState anyState)
                   && anyState.HasTransitionTo(this);
        }
    }
}