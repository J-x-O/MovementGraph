using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.SubStates;
using JescoDev.SmoothBrainStates.Utility;
using UnityEngine;

namespace JescoDev.SmoothBrainStates {
    
    [DefaultExecutionOrder(-5)]
    public class SmoothBrainStateMashine : MonoBehaviour, IStateParent, ISerializationCallbackReceiver {

        public readonly MovementEvents Events = new MovementEvents();
        
        [SerializeField] protected SmoothInOut _connector;
        
        public IEnumerable<State> States => _states.Concat(_connector.Nodes);
        [SerializeReference] protected List<State> _states = new List<State>();
        protected IEnumerable<ExecutableState> _allExecutables => _all.OfType<ExecutableState>();
        protected IEnumerable<State> _all => this.FlattenedStates();

        public ExecutableState CurrentState { get; protected set; }
        public ExecutableState PreviousState { get; protected set; }
        
        [SerializeReference] protected List<ISmoothExtension> _extensions = new();

        protected virtual void Awake() {
            foreach (ExecutableState layer in _allExecutables) layer.Awake();
            foreach (ISmoothExtension extension in _extensions) extension.Setup(this);
        }

        protected virtual void Start() => ActivateState(_connector.ResolveActivation());

        protected virtual void OnDestroy() {
            foreach (ExecutableState layer in _allExecutables) layer.Destroy();
            foreach (ISmoothExtension extension in _extensions) extension.Cleanup(this);
        }

        protected void OnDrawGizmosSelected() {
            if (CurrentState is IGizmosState gizmosState) gizmosState.DrawGizmos(transform);
        }
        
        #region API
        
        public bool SendEvent<T>(Action<T> action) {
            if (CurrentState is not T casted) return false;
            action(casted);
            return true;
        }
        
        public bool IsStateActive<T>() where T : ExecutableState => CurrentState is T;
        public bool IsStateActive(string identifier) => CurrentState.Identifier == identifier;
        
        public bool SetState(string identifier) {
            State state = this.GetState(identifier);
            return ActivateState(state);
        }
        
        /// <inheritdoc cref="SetState{T}"/>
        public bool SetStateFromPath(string identifier) {
            State state = this.GetStateByPath(identifier);
            return ActivateState(state);
        }
        
        /// <summary> Sets the state to a new one of the provided type </summary>
        /// <returns> if the state was activated successfully </returns>
        public bool SetState<T>() where T : ExecutableState {
            State state = this.GetState<T>();
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

        public T GetExtension<T>() where T : class, ISmoothExtension {
            T extension = _extensions.OfType<T>().FirstOrDefault();
            if (extension != null) return extension;
            T newExtension = Activator.CreateInstance<T>();
            _extensions.Add(newExtension);
            return newExtension;
        }

        #endregion
        
        public void OnBeforeSerialize() {
            foreach (State state in States) state?.OnBeforeSerialize();
        }

        public void OnAfterDeserialize() {
            foreach (State layer in States) layer?.OnAfterDeserialize(this);
            foreach (State layer in States) layer?.OnLateDeserialize();
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector] protected Vector2 _cameraPosition;
        [SerializeField, HideInInspector] protected float _cameraZoom = 1;
#endif

        public SmoothBrainStateMashine StateMachine => this;
        public string ResolvePath() => "";
    }
}