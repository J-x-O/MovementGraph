using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace JescoDev.MovementGraph.Layer {
    
    [Serializable]
    public class MovementLayer {
        
        public string Identifier => _identifier;
        [SerializeReference] private string _identifier;

        [SerializeField] private MovementLayerConnector _connector;

        public bool PlayIfInactive => _playIfInactive;
        [SerializeField] private bool _playIfInactive = true;
        
        public bool Autoplay => _autoplay;
        [SerializeField] private bool _autoplay = true;
        
        public LayerComposition Composition => _composition;
        [SerializeField] private LayerComposition _composition;

        public MovementSystem System { get; private set; }
        
        public readonly MovementLayerEvents Events = new MovementLayerEvents();

        public bool IsActive => CurrentState is not MovementStateNull;
        public bool WasActive => PreviousState is not MovementStateNull;

        public IEnumerable<State> States => _states.Concat(_connector.Nodes);
        public IEnumerable<MovementState> MovementStates => _states.OfType<MovementState>();
        [Tooltip("All possible states this character can use")]
        [SerializeReference] private List<State> _states = new List<State>();

        public MovementState PreviousState { get; private set; }
        public MovementState CurrentState { get; private set; }

        internal void Awake(MovementSystem system) {
            System = system;
            _connector.Awake();
            foreach (MovementState state in MovementStates) state.Awake();
        }

        internal void Start() {
            // activate the first valid state and start this layer
            if(_autoplay) Restart();
            else Stop();
        }

        internal MovementDefinition Update() {

            if (CurrentState == null) {
                Debug.LogWarning("No State Active, Reevaluating!");
                Restart();
            }

            return CurrentState?.HandleMovement() ?? MovementDefinition.None;
        }

        internal void OnDestroy() {
            foreach (State state in States) {
                if (state is MovementState movementState) movementState.Destroy();
            }
        }
        
        internal void OnDrawGizmos() => CurrentState?.DrawGizmo();

        #region API

        public void Restart() => ActivateState(_connector.InNode.ResolveActivation());
        public void Stop() => ActivateState(_connector.MovementStateNull);
        
        public void SendEvent<T>(Action<T> action) {
            if(CurrentState is T casted) action(casted);
        }

        /// <summary> Sets the state to a new one of the provided type </summary>
        /// <returns> if the state was activated successfully </returns>
        public bool SetState<T>(bool ignoreActiveCheck = false) where T : MovementState
            => SetState(MovementState.GetName<T>(), ignoreActiveCheck);
        
        /// <inheritdoc cref="SetState{T}"/>
        public bool SetState(string t, bool ignoreActiveCheck = false) {
            if (!TryGetState(t, out State state)) return false;
            if (!ignoreActiveCheck && IsStateActive(t)) return false;
            if (CurrentState != null
                && state.GetInputPorts().Any()
                && !CurrentState.RegularExit.HasTransition(state)) return false;
            
            return ActivateState(state);
        }

        private bool ActivateState(State state) {
            if (!state.ValidActivation()) return false;
            
            // resolve the movement state (this includes a check if it can be activated)
            MovementState result = state.ResolveActivation();
            
            // sometimes this might not find a state which can be activated
            if (result == null) return false;
            
            ActivateState(result);
            return true;
        }
        
        private void ActivateState(MovementState state) {
            
            PreviousState = CurrentState;
            CurrentState = state;
            
            // clear the old one
            if (PreviousState != null) {
                PreviousState.Deactivate();
                Events.InvokeEnd(PreviousState);
                System.Events.InvokeEnd(PreviousState);
            }
            
            CurrentState.Activate();
            Events.InvokeStart(CurrentState);
            System.Events.InvokeStart(CurrentState);
        }

        public void ExitCurrentState() {
            if(CurrentState == null) return;
            ExitCurrentState(CurrentState.RegularExit);
        }
        
        public void ExitCurrentState(MovementPort port) {
            if(CurrentState == null) return;
            if (!CurrentState.GetOutputPorts().Contains(port)) {
                Debug.LogWarning("Trying to exit a state with a port that is not connected to it!");
                return;
            }
            MovementState state = port.FindFirstValidTransition();
            if (state != null) ActivateState(state);
        }

        public T GetState<T>() where T : MovementState
            => GetState(MovementState.GetName<T>()) as T;

        public State GetState(string identifier)
            => States.FirstOrDefault(state => state.Identifier == identifier);
        
        internal State GetStateGuid(string guid)
            => States.FirstOrDefault(state => state.Guid == guid);

        public bool HasState<T>() where T : MovementState => GetState<T>() != null;
        public bool HasState(string identifier) => GetState(identifier) != null;

        public bool TryGetState<T>(out T movementState) where T : MovementState {
            movementState = GetState<T>();
            return movementState != null;
        }
        
        public bool TryGetState(string identifier, out State movementState){
            movementState = GetState(identifier);
            return movementState != null;
        }

        public bool IsStateActive<T>() where T : MovementState => IsStateActive(MovementState.GetName<T>());
        public bool IsStateActive(string identifier) => CurrentState != null && CurrentState.Identifier == identifier;
        
        #endregion

        internal void OnBeforeSerialize() {
            foreach (State state in States) state?.OnBeforeSerialize();
        }

        internal void OnAfterDeserialize() {
            foreach (State state in States) state?.OnAfterDeserialize(this);
            foreach (State state in States) state?.OnLateDeserialize();
        }
    }
}