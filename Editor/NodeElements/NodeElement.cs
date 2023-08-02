using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.MovementEditor;
using Editor.MovementEditor.PropertyUtility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace JescoDev.MovementGraph.Editor.Editor.NodeElements {
    public abstract class NodeElement {

        protected MovementEditorNode Node;
        protected SerializedPropertyState State => Node.State;
        protected MovementGraphView View => Node.View;
        
        protected NodeElement(MovementEditorNode node) {
            Node = node;
        }

        public abstract bool CanBeApplied();
        
        public abstract void Rebuild(List<FieldInfo> fieldInfos);

        
        public bool HasAttribute<T>() where T : Attribute {
            return GetAttributes<T>().Any();
        }
        
        public T GetAttribute<T>() where T : Attribute {
            return GetAttributes<T>().FirstOrDefault();
        }
        
        public IEnumerable<T> GetAttributes<T>() where T : Attribute {
            return Node.StateObject.GetType().GetCustomAttributes<T>();
        }
    }
}