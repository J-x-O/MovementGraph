using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.Layer;
using UnityEngine;

namespace JescoDev.MovementGraph {
    
    [DefaultExecutionOrder(-1)]
    public class MovementSystem : MonoBehaviour, ISerializationCallbackReceiver {

        public readonly MovementEvents Events = new MovementEvents();

        public CustomMovement CustomMovement => _customMovement;
        [SerializeField] private CustomMovement _customMovement;

        public IReadOnlyList<MovementLayer> Layers => _layer;
        [SerializeField] private List<MovementLayer> _layer = new List<MovementLayer>();

        private bool _exitQueued;

        private void Awake() {
            foreach (MovementLayer state in _layer) {
                state.Awake(this);
            }
        }

        private void Start() {
            foreach (MovementLayer layer in _layer) layer.Start();
        }

        private void OnEnable() {
            foreach (MovementLayer layer in _layer) layer.Activate();
        }
        
        private void OnDisable() {
            foreach (MovementLayer layer in _layer) layer.Deactivate();
        }

        private void Update() {
            Vector3 localMovement = Vector3.zero;
            foreach (MovementLayer layer in _layer) {
                if(!layer.IsActive) continue;
                MovementDefinition move = layer.Update();
                switch (move.Context) {
                    case MovementContext.Global:
                        move.Movement = transform.InverseTransformPoint(move.Movement);
                        break;
                    case MovementContext.Teleport:
                        CustomMovement.TeleportTo(move.Movement);
                        continue;
                }

                switch (layer.Composition) {
                    case LayerComposition.Overwrite:
                        localMovement = move.Movement;
                        break;
                    case LayerComposition.Additive:
                        localMovement += move.Movement;
                        break;
                }
            }

            CustomMovement.MoveBy(localMovement);
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
        
        public void OnBeforeSerialize() {
            foreach (MovementLayer layer in _layer) layer.OnBeforeSerialize();
        }

        public void OnAfterDeserialize() {
            foreach (MovementLayer layer in _layer) layer.OnAfterDeserialize();
        }
        
        public MovementLayer GetLayer(string identifier) {
            return Layers.FirstOrDefault(layer => layer.Identifier == identifier);
        }

        public bool TryGetLayer(string identifier, out MovementLayer layer) {
            layer = GetLayer(identifier);
            return layer != null;
        }

#if UNITY_EDITOR
        //[SerializeField] private List<Tuple<string, Color>> _tags;
        [SerializeField, HideInInspector] private Vector2 _cameraPosition;
        [SerializeField, HideInInspector] private float _cameraZoom = 1;
        [SerializeField, HideInInspector] private int _selectedLayer = 0;
#endif
        
    }
}