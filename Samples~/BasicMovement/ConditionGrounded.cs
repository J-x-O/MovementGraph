﻿using System;
using JescoDev.Utility.Condition;
using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    
    [Serializable]
    public class ConditionGrounded : ICondition {

        [SerializeField] private GroundedManager _grounded;
        [SerializeField] private bool _targetState;
        
        public bool Evaluate() => _grounded.Grounded == _targetState;
    }
}