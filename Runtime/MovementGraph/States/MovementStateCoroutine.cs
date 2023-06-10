using System;
using System.Collections;
using System.Collections.Generic;
using JescoDev.MovementGraph.Layer;
using UnityEngine;

namespace Entities.Movement.States {
    public abstract class MovementStateCoroutine : MovementState {

        /// <summary> Overwrite this to still use the normal movement </summary>
        protected Func<MovementDefinition> _handleMovementAction;

        protected Coroutine _routine;
        
        public override void Activate() {
            _routine = Layer.System.StartCoroutine(Coroutine());
        }

        public override void Deactivate() {
            if(_routine != null) Layer.System.StopCoroutine(_routine);
        }

        private IEnumerator Coroutine() {
            yield return RunRoutine();
            QueueRegularExit();
        }

        protected abstract IEnumerator RunRoutine();

        public override MovementDefinition HandleMovement() {
            return _handleMovementAction?.Invoke() ?? MovementDefinition.None;
        }
    }
}