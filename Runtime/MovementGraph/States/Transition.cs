using System;
using UnityEngine;

namespace JescoDev.MovementGraph.States {

    [Serializable]
    public class Transition {

        [field: SerializeReference] public Port From { get; private set; }

        [field: SerializeReference] public Port To { get; private set; }
    }
}