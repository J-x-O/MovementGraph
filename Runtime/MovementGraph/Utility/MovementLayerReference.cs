using System;
using JescoDev.MovementGraph.Layer;
using UnityEngine;

namespace JescoDev.MovementGraph.MovementGraph.Utility {
    
    [Serializable]
    public class MovementLayerReference : ISerializationCallbackReceiver {
        
        [field:SerializeField] public MovementSystem MovementSystem { get; private set; }
        [SerializeField] private string _layerName;
        
        public MovementLayer Layer { get; private set; }

        public void OnBeforeSerialize() => _layerName = Layer.Identifier;

        public void OnAfterDeserialize() => Layer = MovementSystem.GetLayer(_layerName);

        public static implicit operator MovementLayer(MovementLayerReference d) => d.Layer;
    }
}