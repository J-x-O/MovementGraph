using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Movement.States;
using JescoDev.MovementGraph.Layer;
using TMPro;
using UnityEngine;

namespace JescoDev.MovementGraph.Sample.Sample {
    [DefaultExecutionOrder(5)]
    public class StateVisualizer : MonoBehaviour {

        [SerializeField] private MovementSystem _system;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _targetLayer;

        private MovementLayer _layer;
        private void OnEnable() {
            if(_system.TryGetLayer(_targetLayer, out _layer)) {
                _layer.Events.OnAnyStateActivated += HandleStateChange;
            }
        }

        private void OnDisable() {
            _layer.Events.OnAnyStateActivated -= HandleStateChange;
        }

        private IEnumerator Start() {
            yield return null;
            _text.text = _layer.CurrentState.Identifier;
        }

        private void HandleStateChange(MovementState obj) => _text.text = obj.Identifier;
    }
}