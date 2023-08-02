using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.MovementEditor;
using Editor.MovementEditor.PropertyUtility;
using JescoDev.MovementGraph.Editor.Attributes;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JescoDev.MovementGraph.Editor.Editor.NodeElements {
    public class NodeElementHeader : NodeElement {
        
        private SerializedProperty _identifier;
        
        public NodeElementHeader(MovementEditorNode node) : base(node) {}

        public override bool CanBeApplied() {
            if (HasAttribute<HideStateIdentifier>()) {
                return false;
            }
            _identifier = State.FindPropertyRelative("_identifier");
            return _identifier != null;
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            Node.titleContainer.Clear();
            FixedStateIdentifier fixedID = GetAttribute<FixedStateIdentifier>();
            Node.titleContainer.Add(fixedID == null ? CreateHeader() : CreateTitle(fixedID.Identifier));
            fieldInfos.RemoveAll(element => element.Name is "_identifier");
        }
        
        private PropertyField CreateHeader() {
            PropertyField nodeHeader = new PropertyField(_identifier, "");
            nodeHeader.BindProperty(_identifier);
            nodeHeader.RegisterValueChangeCallback(TestForSpecialName);
            nodeHeader.AddToClassList("NodeHeader");
            return nodeHeader;
        }
        
        private void TestForSpecialName(SerializedPropertyChangeEvent evt) {
            Node.mainContainer.RemoveFromClassList("AnyStateContainer");

            if (evt.changedProperty.stringValue == "Any State")
                Node.mainContainer.AddToClassList("AnyStateContainer");
        }

        private VisualElement CreateTitle(string title) {
            VisualElement label = new Label(title);
            label.AddToClassList("NodeTitle");
            return label;
        }
        
        

    }
}