using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Movement;
using Entities.Movement.States;
using UnityEngine;

namespace Movement.States {
    
    [Serializable]
    public abstract class State {

#if UNITY_EDITOR
        [HideInInspector] [SerializeField] protected Vector2 _position;
#endif
        
        public MovementSystem System { get; private set; }
        
        protected Transform _transform => System.transform;
        protected GameObject _gameObject => System.gameObject;
        protected float _movementInput => System.MovementInput;
        
        public virtual void Awake(MovementSystem system) {
            System = system;
        }
        
        public abstract bool ValidActivation();
        
        /// <summary> This function decides if the state can be activated or not </summary>
        /// <returns> The resolved movement state for activation, or null if we cant activate </returns>
        public abstract MovementState ResolveActivation();
    }
}