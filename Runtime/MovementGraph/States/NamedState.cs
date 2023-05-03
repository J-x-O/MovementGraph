using System;
using UnityEngine;
using State = JescoDev.MovementGraph.States.State;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public abstract class NamedState : State {
        
        public string Identifier => _identifier;
        [SerializeField] protected string _identifier;

        public NamedState() : this("") {
            _identifier = GetName(GetType());
        }

        public static string GetName<T> () where T : MovementState => GetName(typeof(T));
        public static string GetName(Type t) => t.Name.Replace("MovementState", "");

        public NamedState(string identifier) {
            _identifier = identifier;
        }
    }
}