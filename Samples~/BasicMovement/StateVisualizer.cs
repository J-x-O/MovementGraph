using System.Collections;
using JescoDev.SmoothBrainStates;
using JescoDev.SmoothBrainStates.States;
using TMPro;
using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    
    public class StateVisualizer : MonoBehaviour {

        [SerializeField] private SmoothBrainStateMashine _system;
        [SerializeField] private TMP_Text _text;

        private void OnEnable() {
            _system.Events.OnAnyStateActivated += HandleStateChange;
        }

        private void OnDisable() {
            _system.Events.OnAnyStateActivated += HandleStateChange;
        }

        private IEnumerator Start() {
            yield return null;
            _text.text = _system.CurrentState.Identifier;
        }

        private void HandleStateChange(ExecutableState obj) {
            _text.text = obj.Identifier;
        }
    }
}