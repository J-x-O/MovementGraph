using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace JescoDev.MovementGraph {
    
    [DefaultExecutionOrder(-5)]
    public class MovementSystem : MonoBehaviour, ISerializationCallbackReceiver {

        public readonly MovementEvents Events = new MovementEvents();

        public CustomMovement CustomMovement => _customMovement;
        [SerializeField] private CustomMovement _customMovement;

        public IReadOnlyList<MovementLayer> Layers => _layer;
        [SerializeField] private List<MovementLayer> _layer = new List<MovementLayer>();

        private bool _exitQueued;

        private void Awake() {
            CustomMovement.MovementSystem = this;
            foreach (MovementLayer state in _layer) state.Awake(this);
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

            List<MovementLayer> activeLayers = new List<MovementLayer>();
            foreach (MovementLayer layer in _layer) {
                if(!layer.IsActive) continue;
                if (layer.Composition == LayerComposition.Overwrite) {
                    activeLayers.RemoveAll(test => !test.PlayIfInactive);
                }
                activeLayers.Add(layer);
            }
            
            foreach (MovementLayer layer in activeLayers) {
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
        
        #region API

        public MovementLayer GetLayer(string identifier) {
            return Layers.FirstOrDefault(layer => layer.Identifier == identifier);
        }

        public bool TryGetLayer(string identifier, out MovementLayer layer) {
            layer = GetLayer(identifier);
            return layer != null;
        }
        
        public void SendEvent<T>(Action<T> action) {
            foreach (MovementLayer layer in _layer) {
                layer.SendEvent(action);
            }
        }

        /// <summary> Sets the state to a new one of the provided type </summary>
        /// <returns> if the state was activated successfully </returns>
        public bool SetState<T>(bool ignoreActiveCheck = false) where T : MovementState
            => SetState(MovementState.GetName<T>(), ignoreActiveCheck);
        
        /// <inheritdoc cref="SetState{T}"/>
        public bool SetState(string identifier, bool ignoreActiveCheck = false) {
            bool any = false;
            ResolvePath(identifier,
                (layer, localId) => any = layer.SetState(localId),
                () => {
                    foreach (MovementLayer layer in _layer) {
                        if(layer.SetState(identifier, ignoreActiveCheck)) any = true;
                    }
                });
            return any;
        }

        public T GetState<T>() where T : MovementState
            => GetState(MovementState.GetName<T>()) as T;

        public State GetState(string identifier) {
            State state = null;
            ResolvePath(identifier,
                (layer, localId) => state = layer.GetState(localId),
                () => {
                    foreach (MovementLayer layer in _layer) {
                        if (!layer.TryGetState(identifier, out State result)) continue;
                        state = result;
                        return;
                    }
                });
            return state;
        }

        private void ResolvePath(string identifier, Action<MovementLayer, string> handleHit, Action handleDefault) {
            Match match = Regex.Match(identifier, @"(.*)\/(.*)");
            if (match.Success) {
                MovementLayer target = GetLayer(match.Groups[0].Value);
                if (target != null) handleHit(target, match.Groups[1].Value);
                else Debug.LogWarning($"Could not find layer {match.Groups[0].Value}");
            }
            else handleDefault();
        }
        

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
        public bool IsStateActive(string identifier) {
            bool any = false;
            ResolvePath(identifier,
                (layer, localId) => any = layer.SetState(localId),
                () => {
                    foreach (MovementLayer layer in _layer) {
                        if(layer.IsStateActive(identifier)) any = true;
                    }
                });
            return any;
        }

        #endregion
        
        public void OnBeforeSerialize() {
            foreach (MovementLayer layer in _layer) layer.OnBeforeSerialize();
        }

        public void OnAfterDeserialize() {
            foreach (MovementLayer layer in _layer) layer.OnAfterDeserialize();
        }

#if UNITY_EDITOR
        //[SerializeField] private List<Tuple<string, Color>> _tags;
        [SerializeField, HideInInspector] private Vector2 _cameraPosition;
        [SerializeField, HideInInspector] private float _cameraZoom = 1;
        [SerializeField, HideInInspector] private int _selectedLayer = 0;
#endif
        
    }
}