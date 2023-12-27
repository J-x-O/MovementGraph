using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.Editor;
using JescoDev.SmoothBrainStates.Editor;
using UnityEditor;
using UnityEditor.UIElements;

namespace JescoDev.SmoothBrainStates.Tags.Editor {
    public class NodeElementTags : NodeElement {
        
        private SerializedProperty _tags;
        
        public NodeElementTags (MovementEditorNode node) : base(node) {}
        
        public override bool CanBeApplied() {
            _tags = State.FindPropertyRelative("_tags");
            return _tags != null;
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            TagView tagView = new TagView(Node.View.Root.GetExtension<SettingsDrawerTags>());
            tagView.BindProperty(_tags);
            Node.mainContainer.Insert(1, tagView);
            fieldInfos.RemoveAll(element => element.Name is "_tags");
        }
    }
}