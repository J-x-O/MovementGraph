using System;
using UnityEngine;

namespace JescoDev.MovementGraph.States {

    [Serializable]
    public class Transition {

        [field: SerializeReference] public Port From { get; private set; }

        [field: SerializeReference] public Port To { get; private set; }

        public Transition() { }
        
        public Transition(Port from, Port to) {
            From = from;
            To = to;
        }

        public Port NotMe(Port me) => From != me ? From : To;
    }
}