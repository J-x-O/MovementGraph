using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;


namespace Editor.MovementEditor {

    public abstract class BaseNode : Node {
        
        public SerializedProperty State { get; protected set; }
        
        public State StateObject { get; protected set; }

        protected BaseNode(SerializedProperty state, State stateObject) {
            State = state;
            StateObject = stateObject;

            Rebuild();
        }

        public void Rebuild(SerializedProperty property) {
            State = property;
            Rebuild();
        }
        
        public void Rebuild() {
            List<FieldInfo> fieldInfos = StateObject.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderBy(field => field.MetadataToken)
                .ToList();
            
            fieldInfos.RemoveAll(element => element.Name is "_transitions" or "_position");
            
            Rebuild(fieldInfos);
            
            mainContainer.AddToClassList("NodeMainContainer");
            extensionContainer.AddToClassList("NodeExtensionContainer");
            RefreshExpandedState();
        }

        protected abstract void Rebuild(List<FieldInfo> fieldInfos);
    }
    
    
}