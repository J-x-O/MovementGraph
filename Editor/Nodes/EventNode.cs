using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entities.Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    
    [Serializable]
    public class EventNode : NamedNode {
        
        public new EventState StateObject { get; protected set; }

        public EventNode(MovementGraphView view, SerializedProperty state, EventState stateObject) : base(view, state, stateObject) {
            StateObject = stateObject;
        }

        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            RebuildHeader(fieldInfos);
            NodeHeader.RegisterValueChangeCallback(TestForSpecialName);
        }

        private void TestForSpecialName(SerializedPropertyChangeEvent evt) {
            mainContainer.RemoveFromClassList("ReevaluateContainer");
            mainContainer.RemoveFromClassList("AnyStateContainer");

            switch (evt.changedProperty.stringValue) {
                case "Reevaluate":
                    mainContainer.AddToClassList("ReevaluateContainer");
                    break;
                case "Any State":
                    mainContainer.AddToClassList("AnyStateContainer");
                    break;
            }
        }
    }
}