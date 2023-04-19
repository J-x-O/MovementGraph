using System;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class Transition {
        public State Target => _target;
        [SerializeReference] private State _target;
    }
}