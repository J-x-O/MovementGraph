using System;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace JescoDev.MovementGraph.MovementGraph.Utility {
    
    [Serializable]
    public class MovementStateReference<T> : ISerializationCallbackReceiver where T : MovementState {
        
        [field:SerializeField] public MovementSystem MovementSystem { get; private set; }
        [SerializeField] private string _statePath;
        
        public T MovementState { get; private set; }

        public void OnBeforeSerialize() {
            if(MovementState != null) _statePath = MovementState.Layer.Identifier + "/" + MovementState.Identifier;
        }

        public void OnAfterDeserialize() {
            if(MovementSystem != null) MovementState = MovementSystem.GetState(_statePath) as T;
        }

        public static implicit operator T(MovementStateReference<T> d) => d.MovementState;
    }
}