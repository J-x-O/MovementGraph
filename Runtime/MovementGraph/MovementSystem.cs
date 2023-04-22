using System;
using System.Collections.Generic;
using Entities.Movement.States;
using Gameplay;
using Gameplay.Movement.Layer;
using GameProgramming.Utility.TypeBasedEventSystem;
using Movement;
using Movement.States;
using Player.Movement;
using TNRD;
using UnityEngine;

namespace Entities.Movement {
    [DefaultExecutionOrder(-1)]
    public class MovementSystem : MonoBehaviour {

        public readonly MovementEvents Events = new MovementEvents();

        /// <summary> The currently active Movement State of the state machine </summary>
        public MovementState CurrentState { get; private set; }

        /// <summary> The Movement State of the state machine  which was active before the current one </summary>
        public MovementState PreviousState { get; private set; }

        public CharacterController CharController => _charController;
        [SerializeField] private CharacterController _charController;

        public GroundedManager FloorManager => _floorManager;
        [SerializeField] private GroundedManager _floorManager;

        [Tooltip("All possible states this character can use")] [SerializeReference]
        private List<MovementLayer> _states = new List<MovementLayer>();

        public float MovementInput => InputSource?.MovementValue ?? 0;
        public IMovementSource InputSource => _source.Value;
        [SerializeField] private SerializableInterface<IMovementSource> _source;
        
        private bool _exitQueued;

        private void Awake() {
            foreach (MovementLayer state in _states) {
                state.Awake(this);
            }
            CharController.enabled = false;
        }

        // wait for everything to initialize
        private void Start() {
            foreach (MovementLayer state in _states) {
                state.QueueExit(this);
            }
            CharController.enabled = true;
        }

        private void Update() {

            if (_exitQueued) {
                _exitQueued = false;
                ExitCurrentState();
            }
            
            if (CurrentState == null) {
                Debug.LogWarning("No State Active, Reevaluating!");
                ExitCurrentState();
            }

            CurrentState?.HandleMovement(MovementInput);
        }

        private void OnDestroy() {
            foreach (NamedState baseState in _stateDictionary.Values) {
                if (baseState is MovementState state) state.Destroy();
            }
        }

        private void OnDrawGizmos() => CurrentState?.DrawGizmo();

#if UNITY_EDITOR
        //[SerializeField] private List<Tuple<string, Color>> _tags;
        [SerializeField, HideInInspector] private Vector2 _cameraPosition;
        [SerializeField, HideInInspector] private float _cameraZoom = 1;
#endif
    }
}