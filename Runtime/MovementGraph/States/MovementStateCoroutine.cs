using System;
using System.Collections;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    public abstract class MovementStateCoroutine : MovementState {

        /// <summary> Overwrite this to still use the normal movement </summary>
        protected Action<float> _handleMovementAction;

        protected Coroutine _routine;
        
        public override void Activate() {
            _routine = System.StartCoroutine(Coroutine());
        }

        public override void Deactivate() {
            if(_routine != null) System.StopCoroutine(_routine);
        }

        private IEnumerator Coroutine() {
            yield return RunRoutine();
            System.QueueReevaluation();
        }

        protected abstract IEnumerator RunRoutine();

        public override void HandleMovement(float input) {
            _handleMovementAction?.Invoke(input);
        }
    }
}