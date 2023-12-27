using System;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Utility {
    
    [Serializable]
    public class MovementStateReference<T> : ISerializationCallbackReceiver where T : ExecutableState {
        
        [field:SerializeField] public SmoothBrainStateMashine SmoothBrainStateMashine { get; private set; }
        [SerializeField] private string _statePath;
        
        public T MovementState { get; private set; }

        public void OnBeforeSerialize() {
            if(MovementState != null) _statePath = MovementState.Parent.ResolvePath() + "/" + MovementState.Identifier;
        }

        public void OnAfterDeserialize() {
            if(SmoothBrainStateMashine != null) MovementState = SmoothBrainStateMashine.GetState(_statePath) as T;
        }

        public static implicit operator T(MovementStateReference<T> d) => d.MovementState;
    }
}