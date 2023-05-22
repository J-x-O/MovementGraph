using System;
using System.Collections.Generic;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using JescoDev.Utility.Condition;
using UnityEngine;

namespace Entities.Movement.States {
    
    [Serializable]
    public abstract class MovementState : NamedState {

        [field: SerializeField, InputPort] public MovementPort InputPort { get; private set; }
        [field: SerializeField, OutputPort] public MovementPort RegularExit { get; private set; }
        [field: SerializeField, OutputPort] public MovementPort EventExit { get; private set; }

        [Tooltip("The Condition that needs to be true so this can be activated")]
        [SubclassSelector] [SerializeReference] protected ICondition _activationCondition;

        public IReadOnlyList<string> Tags => _tags;
        [HideInInspector] [SerializeField] private List<string> _tags = new List<string>();
        
        public override void Awake(MovementLayer layer) {
            base.Awake(layer);
            Awake();
        }
        
        protected virtual void Awake() {}
        
        public virtual void Destroy() {}

        public virtual void DrawGizmo() {}
        
        public virtual bool CanBeActivated() => _activationCondition == null || _activationCondition.Evaluate();

        public virtual void Activate() {}
        
        public virtual void Deactivate() {}

        public abstract Vector3 HandleMovement(float input);
        
        public bool HasTag(string tag) => _tags.Contains(tag);

        public override bool ValidActivation() => InputPort.HasActiveTransition(true);
        
        /// <summary> It is important that only an state which can be activated results in a valid return </summary>
        public override MovementState ResolveActivation(MovementPort incomingPort = null) {
            return CanBeActivated() ? this : null;
        }
        
        public void QueueRegularExit() => Layer.QueueExit(RegularExit);
    }
}