using System.Collections.Generic;
using System.Reflection;
using Editor.MovementEditor;
using UnityEditor;
using UnityEditor.UIElements;

namespace JescoDev.MovementGraph.Editor.Editor.NodeElements {
    public class NodeElementVariables : NodeElement {

        public NodeElementVariables(MovementEditorNode node) : base(node) {
            
        }
        
        public override bool CanBeApplied() {
            return true;
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            Node.extensionContainer.Clear();
            foreach (FieldInfo fieldInfo in fieldInfos) {
                SerializedProperty property = Node.State.FindPropertyRelative(fieldInfo.Name);
                if (property == null) continue;
                
                PropertyField field = new PropertyField(property);
                field.BindProperty(property);
                Node.extensionContainer.Add(field);
            }
            fieldInfos.Clear();
        }
    }
}