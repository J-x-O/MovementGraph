using System.Collections.Generic;
using System.Reflection;
using Entities.Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class RedirectNode : BaseNode{

        public RedirectNode(SerializedProperty state, RedirectState stateObject) : base(state, stateObject) { }

        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            this.RebuildInput("");
            this.RebuildOutput("");
            mainContainer.AddToClassList("RedirectNode");
            titleContainer.Clear();
        }
    }
}