using System;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.MovementGraph.States;
using JescoDev.Utility.EventUtility.EventUtility;

namespace JescoDev.MovementGraph.Layer {
    public class MovementLayerEvents {
        
        public event Action OnLayerStart;
        public event Action OnLayerStop;
    
        public event Action<MovementState> OnAnyStateActivated;
        public event Action<MovementState> OnAnyStateDeactivated;

        public IReadOnlyVarBasedEventSystem<string, MovementState> ID => _id;
        private readonly VarBasedEventSystem<string, MovementState> _id = new();
        public IReadOnlyVarBasedEventSystem<string, MovementState> Tags => _tags;
        private readonly VarBasedEventSystem<string, MovementState> _tags = new();
    
        internal void InvokeStart(MovementState newState) {
            OnAnyStateActivated.TryInvoke(newState);
            string identifier = newState.Identifier;
            _id.InvokeVarBasedEventStart(identifier, newState);
            foreach (string tag in newState.Tags) {
                _tags.InvokeVarBasedEventStart(tag, newState);
            }
        
            if(newState.Layer.IsActive && !newState.Layer.WasActive) {
                OnLayerStart.TryInvoke();
            }
        }
    
        internal void InvokeEnd(MovementState oldState) {
            OnAnyStateDeactivated.TryInvoke(oldState);
            if(oldState == null) return;
            string identifier = oldState.Identifier;
            _id.InvokeVarBasedEventEnd(identifier, oldState);
            foreach (string tag in oldState.Tags) {
                _tags.InvokeVarBasedEventEnd(tag, oldState);
            }
        
            if(!oldState.Layer.IsActive && oldState.Layer.WasActive) {
                OnLayerStop.TryInvoke();
            }
        }
    }
}