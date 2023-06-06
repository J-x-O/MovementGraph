using System.Collections.Generic;
using System.Reflection;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class RedirectNode : BaseNode{

        public RedirectNode(MovementGraphView view, SerializedProperty state, RedirectState stateObject) : base(view, state, stateObject) { }

        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            mainContainer.AddToClassList("RedirectNode");
            titleContainer.Clear();
        }
    }
}