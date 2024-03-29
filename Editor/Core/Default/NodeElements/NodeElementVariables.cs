using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.Editor;
using UnityEditor;
using UnityEditor.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public class NodeElementVariables : NodeElement {

        public NodeElementVariables(MovementEditorNode node) : base(node) {
            
        }
        
        public override bool CanBeApplied() {
            return true;
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
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