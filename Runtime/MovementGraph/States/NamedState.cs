using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using State = Movement.States.State;

namespace Entities.Movement.States {
    
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

        public CollisionFlags MovePosition(Vector3 position) => MoveDirection(position - Transform.position);

        public CollisionFlags MoveDirection(Vector3 direction) {
            //TODO: Auslagern into System
            CollisionFlags flags = Layer.System.CharController.Move(direction);
            Vector3 pos = Transform.position;
            pos.z = 0;
            Transform.position = pos;
            return flags;
        }
    }
}