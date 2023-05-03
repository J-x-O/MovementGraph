using System;
using System.Collections.Generic;
using JescoDev.Utility.Condition;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public abstract class MovementState : NamedState {

        [Tooltip("The Condition that needs to be true so this can be activated")]
        [SubclassSelector] [SerializeReference] protected ICondition _activationCondition;

        public IReadOnlyList<string> Tags => _tags;
        [HideInInspector] [SerializeField] private List<string> _tags = new List<string>();
        
        public override void Awake(MovementSystem system) {
            base.Awake(system);
            Awake();
        }
        
        protected virtual void Awake() {}
        
        public virtual void Destroy() {}

        public virtual void DrawGizmo() {}
        
        public virtual bool CanBeActivated() => _activationCondition == null || _activationCondition.Evaluate();

        public virtual void Activate() {}
        
        public virtual void Deactivate() {}

        public abstract void HandleMovement();
        
        public bool HasTag(string tag) => _tags.Contains(tag);

        public override bool ValidActivation() => HasTransition(true);
        
        /// <summary> It is important that only an state which can be activated results in a valid return </summary>
        public override MovementState ResolveActivation() {
            return CanBeActivated() ? this : null;
        }
    }
}