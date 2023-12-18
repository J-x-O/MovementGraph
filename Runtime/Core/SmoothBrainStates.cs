using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates {
    
    [DefaultExecutionOrder(-5)]
    public class SmoothBrainStates : MonoBehaviour, IStateParent, ISerializationCallbackReceiver {

        public readonly MovementEvents Events = new MovementEvents();

        public IEnumerable<State> States => _states;
        [SerializeField] protected List<State> _states = new List<State>();

        public IEnumerable<SmoothPort> InputPorts => Enumerable.Empty<SmoothPort>();
        public IEnumerable<SmoothPort> OutputPorts => Enumerable.Empty<SmoothPort>();
        
        protected IEnumerable<ExecutableState> _executableStates => _states.OfType<ExecutableState>();
        protected IEnumerable<State> _flattened => (this as IStateParent).FlattenedStates();

        public ExecutableState CurrentState { get; protected set; }
        public ExecutableState PreviousState { get; protected set; }

        protected void Awake() {
            foreach (ExecutableState layer in _executableStates) layer.Awake();
        }
        
        protected void OnDestroy() {
            foreach (ExecutableState layer in _executableStates)
                layer.Destroy();
        }

        protected void OnDrawGizmosSelected() {
            foreach (ExecutableState layer in _executableStates)
                layer.Destroy();
        }
        
        #region API
        
        public bool SendEvent<T>(Action<T> action) {
            if (CurrentState is not T casted) return false;
            action(casted);
            return true;
        }
        
        public bool SetState(string identifier) {
            State state = (this as IStateParent).GetState(identifier);
            return ActivateState(state);
        }
        
        /// <inheritdoc cref="SetState{T}"/>
        public bool SetStateFromPath(string identifier) {
            State state = (this as IStateParent).GetStateByPath(identifier);
            return ActivateState(state);
        }
        
        /// <summary> Sets the state to a new one of the provided type </summary>
        /// <returns> if the state was activated successfully </returns>
        public bool SetState<T>() where T : ExecutableState {
            State state = (this as IStateParent).GetState<T>();
            return ActivateState(state);
        }

        private bool ActivateState(State state) {
            
            bool requiresConnection = state.GetInputPorts().Any() && CurrentState != state;
            SmoothPort found = null;
            if (requiresConnection && CurrentState != null) {
                found = CurrentState.GetOutputPorts()
                    .FirstOrDefault(port => port.HasTransition(state));
                if (found == null) return false;
            }
            
            if (!state.CanBeActivated()) return false;
            
            // resolve the movement state (this includes a check if it can be activated)
            ExecutableState result = state.ResolveActivation(found);
            
            // sometimes this might not find a state which can be activated
            if (result == null) return false;
            if (!result.CanBeReactivated && CurrentState == state) return false;
            
            ActivateState(result);
            return true;
        }
        
        private void ActivateState(ExecutableState state) {
            PreviousState = CurrentState;
            CurrentState = state;
            
            // clear the old one
            if (PreviousState != null) {
                PreviousState.OnDeactivate();
                Events.InvokeEnd(PreviousState);
            }
            
            CurrentState.OnActivate();
            Events.InvokeStart(CurrentState);
        }
        
        public void ExitCurrentState(SmoothPort port = null) {
            if(CurrentState == null) return;
            port ??= CurrentState.RegularExit;
            if (!CurrentState.GetOutputPorts().Contains(port)) {
                Debug.LogWarning("Trying to exit a state with a port that is not connected to it!");
                return;
            }
            ExecutableState state = port.FindFirstValidTransition();
            if (state != null) ActivateState(state);
        }

        #endregion
        
        public void OnBeforeSerialize() {
            foreach (State layer in _states) layer.OnBeforeSerialize();
        }

        public void OnAfterDeserialize() {
            foreach (State layer in _states) layer.OnAfterDeserialize(this);
        }

#if UNITY_EDITOR
        //[SerializeField] protected List<Tuple<string, Color>> _tags;
        [SerializeField, HideInInspector] protected Vector2 _cameraPosition;
        [SerializeField, HideInInspector] protected float _cameraZoom = 1;
#endif

        public SmoothBrainStates StateMachine => this;
        public string ResolvePath() => "";
    }
}