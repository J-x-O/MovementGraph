using System;
using System.Collections;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Movement {
    public abstract class MovementStateCoroutine : ExecutableState, IMovementState {

        /// <summary> Overwrite this to still use the normal movement </summary>
        protected Func<MovementDefinition> _handleMovementAction;

        protected Coroutine _routine;
        
        protected MovementStateCoroutine(string identifier) : base(identifier) { }
        
        protected override void OnActivate() {
            _routine = StateMashineMachine.StartCoroutine(Coroutine());
        }

        protected override void OnDeactivate() {
            if(_routine != null) StateMashineMachine.StopCoroutine(_routine);
        }

        private IEnumerator Coroutine() {
            yield return RunRoutine();
            ExitCurrentState();
        }

        protected abstract IEnumerator RunRoutine();

        public MovementDefinition HandleMovement() {
            return _handleMovementAction?.Invoke() ?? MovementDefinition.None;
        }
    }
}