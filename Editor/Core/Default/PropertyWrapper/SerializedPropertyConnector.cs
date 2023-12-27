using System.Collections.Generic;
using JescoDev.MovementGraph.Editor;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEditor;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Editor {
    public class SerializedPropertyConnector : SerializedPropertyExtension {
        
        public SerializedPropertyState InNode => new SerializedPropertyState(_inNodeProperty);
        public SerializedPropertyState OutNode => new SerializedPropertyState(_outNodeProperty);
        
        private readonly SerializedProperty _inNodeProperty;
        private readonly SerializedProperty _outNodeProperty;
        
        public SerializedPropertyConnector(SerializedProperty property) : base(property) {
            if (property == null) return;
            _inNodeProperty = property.FindPropertySave("_inNode");
            _inNodeProperty.managedReferenceValue ??= new SubContainerIn();
            _outNodeProperty = property.FindPropertySave("_outNode");
            _outNodeProperty.managedReferenceValue ??= new SubContainerOut();

            property.serializedObject.ApplyModifiedProperties();
        }
        
        public void ResetInOut() {
            _inNodeProperty.managedReferenceValue = new SubContainerIn();
            _outNodeProperty.managedReferenceValue = new SubContainerOut();
            OutNode.Position = new Vector2(250, 0);
        }
        
        public IEnumerable<SerializedPropertyState> GetStates() {
            return new List<SerializedPropertyState> { InNode, OutNode };
        }

        public override bool IsValid() {
            return Property != null && InNode.IsValid() && OutNode.IsValid();
        }
    }
}