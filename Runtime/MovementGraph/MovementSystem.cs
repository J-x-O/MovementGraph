using System.Collections.Generic;
using JescoDev.MovementGraph.States;
using TNRD;
using UnityEngine;

namespace JescoDev.MovementGraph {
    [DefaultExecutionOrder(-1)]
    public class MovementSystem : MonoBehaviour {

        public readonly MovementEvents Events = new MovementEvents();

        /// <summary> The currently active Movement State of the state machine </summary>
        public MovementState CurrentState { get; private set; }

        /// <summary> The Movement State of the state machine  which was active before the current one </summary>
        public MovementState PreviousState { get; private set; }

        public CharacterController CharController => _charController;
        [SerializeField] private CharacterController _charController;

        [Tooltip("All possible states this character can use")] [SerializeReference]
        private List<State> _states = new List<State>();
        private readonly Dictionary<string, NamedState> _stateDictionary = new Dictionary<string, NamedState>();

        public float MovementInput => InputSource?.MovementValue ?? 0;
        public IMovementSource InputSource => _source.Value;
        [SerializeField] private SerializableInterface<IMovementSource> _source;
        
        private bool _reevaluationQueued;

        private void Awake() {
            foreach (State state in _states) {
                state.Awake(this);
                
                if(state is not NamedState namedState) continue;
                _stateDictionary.Add(namedState.Identifier, namedState);
            }
            CharController.enabled = false;
        }

        // wait till the floor manager run once 
        private void Start() {
            ReevaluateStates();
            CharController.enabled = true;
        }

        private void Update() {

            if (_reevaluationQueued) {
                _reevaluationQueued = false;
                ReevaluateStates();
            }
            
            if (CurrentState == null) {
                Debug.LogWarning("No State Active, Reevaluating!");
                ReevaluateStates();
            }

            CurrentState?.HandleMovement(MovementInput);
        }

        private void OnDestroy() {
            foreach (NamedState baseState in _stateDictionary.Values) {
                if (baseState is MovementState state) state.Destroy();
            }
        }

        private void OnDrawGizmos() => CurrentState?.DrawGizmo();

        #region API

        /// <summary> Sets the state to a new one of the provided type </summary>
        /// <returns> if the state was activated successfully </returns>
        public bool SetState<T>(bool ignoreActiveCheck = false) where T : MovementState
            => SetState(NamedState.GetName<T>(), ignoreActiveCheck);
        
        /// <inheritdoc cref="SetState{T}"/>
        public bool SetState(string t, bool ignoreActiveCheck = false) {
            if (!TryGetState(t, out NamedState state)) return false;
            if (!ignoreActiveCheck && IsStateActive(t)) return false;
            return ActivateState(state);
        }

        private bool ActivateState(NamedState state) {
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
            }
            
            CurrentState.Activate();
            Events.InvokeStart(CurrentState);
        }

        public void QueueReevaluation() => _reevaluationQueued = true;

        private void ReevaluateStates() {
            if (!TryGetState("Reevaluate", out NamedState state)) return;
            ActivateState(state);
        }

        public T GetState<T>() where T : MovementState {
            string stateName = NamedState.GetName<T>();
            return _stateDictionary.ContainsKey(stateName)
                ? _stateDictionary[stateName] as T
                : null;
        }

        public NamedState GetState(string identifier) {
            return _stateDictionary.ContainsKey(identifier)
                ? _stateDictionary[identifier]
                : null;
        }
        
        public bool HasState<T>() where T : MovementState => GetState<T>() != null;
        public bool HasState(string identifier) => GetState(identifier) != null;

        public bool TryGetState<T>(out T movementState) where T : MovementState {
            movementState = GetState<T>();
            return movementState != null;
        }
        
        public bool TryGetState(string identifier, out NamedState movementState){
            movementState = GetState(identifier);
            return movementState != null;
        }

        public bool IsStateActive<T>() where T : MovementState => IsStateActive(NamedState.GetName<T>());
        public bool IsStateActive(string identifier) => CurrentState != null && CurrentState.Identifier == identifier;
        
        #endregion

#if UNITY_EDITOR
        //[SerializeField] private List<Tuple<string, Color>> _tags;
        [SerializeField, HideInInspector] private Vector2 _cameraPosition;
        [SerializeField, HideInInspector] private float _cameraZoom = 1;
#endif
    }
}