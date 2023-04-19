using System;
using System.Collections.Generic;
using Entities.Movement;
using Entities.Movement.States;
using Movement;
using Movement.States;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class MovementLayer : IStateLoop {
        
        public LayerInOut Interface => _interface;
        [SerializeReference] private LayerInOut _interface;

        public MovementSystem System { get; private set; }
        
        public readonly MovementEvents Events = new MovementEvents();
        
        public IReadOnlyList<State> States => _states;
        [Tooltip("All possible states this character can use")] [SerializeReference]
        private List<State> _states = new List<State>();
        private readonly Dictionary<string, NamedState> _stateDictionary = new Dictionary<string, NamedState>();

        private bool _exitQueued;
        
        public MovementState PreviousState { get; private set; }
        public MovementState CurrentState { get; private set; }
        
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

        public void QueueExit() => _exitQueued = true;

        private void ExitCurrentState() {
            MovementState state = CurrentState.RegularExit.FindFirstValidTransition();
            if(state != null) ActivateState(state);
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

        
    }
}