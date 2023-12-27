using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.Editor;
using JescoDev.SmoothBrainStates.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public class NodeElementHeader : NodeElement {
        
        private SerializedProperty _identifier;
        
        public NodeElementHeader(MovementEditorNode node) : base(node) {}

        public override bool CanBeApplied() {
            _identifier = State.FindPropertyRelative("_identifier");
            return _identifier != null;
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            if (!HasAttribute<SmoothStateHideIdentifier>()) {
                SmoothStateFixedIdentifier smoothStateFixedID = GetAttribute<SmoothStateFixedIdentifier>();
                VisualElement title = smoothStateFixedID == null
                    ? CreateHeader()
                    : CreateTitle(smoothStateFixedID.Identifier);
                Node.titleContainer.Insert(0, title);
            }
            
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