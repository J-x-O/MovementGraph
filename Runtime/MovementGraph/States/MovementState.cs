using System;
using System.Collections.Generic;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.StateTransition;
using JescoDev.Utility.Condition;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public abstract class MovementState : State {

        [field: SerializeField, InputPort] public MovementPort InputPort { get; protected set; }
        [field: SerializeField, OutputPort] public MovementPort RegularExit { get; protected set; }

        [Tooltip("The Condition that needs to be true so this can be activated")]
        [SubclassSelector] [SerializeReference] protected ICondition _activationCondition;

        public IReadOnlyList<string> Tags => _tags;
        [HideInInspector] [SerializeField] private List<string> _tags = new List<string>();

        public MovementState() : base("") {
            _identifier = GetName(GetType());
        }
        
        public static string GetName<T> () where T : MovementState => GetName(typeof(T));
        public static string GetName(Type t) => t.Name.Replace("MovementState", "");
        
        public virtual void Awake() { }
        
        public virtual void Destroy() {}

        public virtual void DrawGizmo() {}
        
        public virtual bool CanBeActivated() => _activationCondition == null || _activationCondition.Evaluate();

        public virtual void Activate() {}
        
        public virtual void Deactivate() {}

        public abstract MovementDefinition HandleMovement();
        
        public bool HasTag(string tag) => _tags.Contains(tag);

        public override bool ValidActivation() => CanBeActivated();
        
        /// <summary> It is important that only an state which can be activated results in a valid return </summary>
        public override MovementState ResolveActivation(MovementPort incomingPort = null) {
            return this;
        }
        
        public void ExitCurrentState() => Layer.ExitCurrentState(RegularExit);
    }
}