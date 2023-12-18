using System;
using JescoDev.MovementGraph.Layer;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.MovementGraph.MovementGraph.Utility {
    
    [Serializable]
    public class MovementLayerReference : ISerializationCallbackReceiver {
        
        [field:SerializeField] public SmoothBrainStates.Core.SmoothBrainStates SmoothBrainStates { get; private set; }
        [SerializeField] private string _layerName;
        
        public StateParent Layer { get; private set; }

        public void OnBeforeSerialize() {
            _layerName = Layer?.Identifier;
        }

        public void OnAfterDeserialize() {
            if(SmoothBrainStates != null) Layer = SmoothBrainStates.GetLayer(_layerName);
        }

        public static implicit operator StateParent(MovementLayerReference d) => d.Layer;
    }
}