using System;
using UnityEngine;

namespace JescoDev.MovementGraph.States {

    [Serializable]
    public class Transition {

        [field: SerializeReference] public MovementPort From { get; private set; }

        [field: SerializeReference] public MovementPort To { get; private set; }

        public Transition() { }
        
        public Transition(MovementPort from, MovementPort to) {
            From = from;
            To = to;
        }

        public MovementPort NotMe(MovementPort me) => From != me ? From : To;
    }
}