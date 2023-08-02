using System.Collections.Generic;
using System.Reflection;
using Editor.MovementEditor;
using UnityEditor;
using UnityEditor.UIElements;

namespace JescoDev.MovementGraph.Editor.Editor.NodeElements {
    public class NodeElementTags : NodeElement {
        
        private SerializedProperty _tags;
        
        public NodeElementTags (MovementEditorNode node) : base(node) {}
        
        public override bool CanBeApplied() {
            _tags = State.FindPropertyRelative("_tags");
            return _tags != null;
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            TagView tagView = new TagView();
            tagView.BindProperty(_tags);
            Node.titleContainer.Add(tagView);
            
            fieldInfos.RemoveAll(element => element.Name is "_tags");
        }
    }
}