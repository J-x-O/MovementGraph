using System.Collections.Generic;
using System.Reflection;
using Editor.MovementEditor.PropertyUtility;
using Gameplay.Movement.Layer;
using Movement.States;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class LayerInNode : BaseNode {

        public LayerInNode(MovementGraphView view, SerializedPropertyState state, LayerIn stateObject)
            : base(view, state, stateObject) {
        }
        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            titleContainer.Clear();
            VisualElement label = new Label("Layer In");
            label.AddToClassList("NodeTitle");
            titleContainer.Add(label);
        }
    }
    
    public class LayerOutNode : BaseNode {

        public LayerOutNode(MovementGraphView view, SerializedPropertyState state, LayerOut stateObject)
            : base(view, state, stateObject) {
        }
        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            titleContainer.Clear();
            VisualElement label = new Label("Layer Out");
            label.AddToClassList("NodeTitle");
            titleContainer.Add(label);
        }
    }
}