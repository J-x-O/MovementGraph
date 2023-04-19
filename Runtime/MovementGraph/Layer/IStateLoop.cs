using System.Collections.Generic;
using Entities.Movement.States;
using Movement.States;
using UnityEngine;

namespace Gameplay.Movement.Layer {
    public interface IStateLoop {
        
        public LayerInOut Interface { get; }

        public IReadOnlyList<State> States { get; }
    }
}