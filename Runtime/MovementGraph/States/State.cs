using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entities.Movement;
using Entities.Movement.States;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace Movement.States {
    
    [Serializable]
    public abstract class State {

#if UNITY_EDITOR
        [HideInInspector] [SerializeField] protected Vector2 _position;
#endif
        
        public MovementLayer Layer { get; private set; }
        
        public Transform Transform => Layer.System.transform;
        public GameObject GameObject => Layer.System.gameObject;
        protected float _movementInput => Layer.System.MovementInput;

        public virtual void Awake(MovementLayer layer) {
            Layer = layer;
        }
        
        public abstract bool ValidActivation();
        
        /// <summary> This function decides if the state can be activated or not </summary>
        /// <returns> The resolved movement state for activation, or null if we cant activate </returns>
        public abstract MovementState ResolveActivation(Port incomingPort = null);

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.NonPublic |
                                                  System.Reflection.BindingFlags.Public;
        
        public IEnumerable<Port> GetAllPorts() {
            return GetType()
                .GetFields(BindingFlags)
                .Select(field => field.GetValue(this))
                .OfType<Port>();
        }
        
        private IEnumerable<Port> GetFilteredPorts(Func<FieldInfo ,bool> predicate) {
            return GetType()
                .GetFields(BindingFlags)
                .Where(predicate)
                .Select(field => field.GetValue(this))
                .OfType<Port>();
        }
        
        public IEnumerable<Port> GetInputPorts() {
            return GetFilteredPorts(field => field.GetCustomAttribute<PortType>()?.IsInput ?? false);
        }
        
        public IEnumerable<Port> GetOutputPorts() {
            return GetFilteredPorts(field => field.GetCustomAttribute<PortType>()?.IsOutput ?? false);
        }
    }
}