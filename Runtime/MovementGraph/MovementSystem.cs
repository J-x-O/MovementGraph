using System;
using System.Collections.Generic;
using Entities.Movement.States;
using Gameplay;
using Gameplay.Movement.Layer;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.MovementGraph.Layer;
using Movement;
using Movement.States;
using Player.Movement;
using TNRD;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Movement {
    [DefaultExecutionOrder(-1)]
    public class MovementSystem : MonoBehaviour {

        public readonly MovementEvents Events = new MovementEvents();

        public CharacterController CharController => _charController;
        [SerializeField] private CharacterController _charController;

        public GroundedManager FloorManager => _floorManager;
        [SerializeField] private GroundedManager _floorManager;

        [Tooltip("All possible states this character can use")]
        [SerializeField] private List<MovementLayer> _layer = new List<MovementLayer>();

        public float MovementInput => InputSource?.MovementValue ?? 0;
        public IMovementSource InputSource => _source.Value;
        [SerializeField] private SerializableInterface<IMovementSource> _source;
        
        private bool _exitQueued;

        private void Awake() {
            foreach (MovementLayer state in _layer) {
                state.Awake(this);
            }
            CharController.enabled = false;
        }

        // wait for everything to initialize
        private void Start() {
            foreach (MovementLayer layer in _layer) {
                layer.Restart();
            }
            CharController.enabled = true;
        }

        private void Update() {
            Vector3 movement = Vector3.zero;
            foreach (MovementLayer layer in _layer) {
                //TODO account for layer mode
                movement += layer.Update(MovementInput);
            }

            CharController.Move(movement);
        }

        private void OnDestroy() {
            foreach (MovementLayer layer in _layer) {
                layer.OnDestroy();
            }
        }

        private void OnDrawGizmos() {
            foreach (MovementLayer layer in _layer) {
                layer.OnDrawGizmos();
            }
        }

#if UNITY_EDITOR
        //[SerializeField] private List<Tuple<string, Color>> _tags;
        [SerializeField, HideInInspector] private Vector2 _cameraPosition;
        [SerializeField, HideInInspector] private float _cameraZoom = 1;
#endif
    }
}