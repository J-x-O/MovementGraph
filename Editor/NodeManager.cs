using System;
using System.Collections.Generic;
using System.Linq;
using Editor.MovementEditor.PropertyUtility;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public partial class NodeManager {
        
        public readonly SerializedProperty StatesProperty;
        public readonly MovementGraphView _view;
        public readonly List<BaseNode> _nodes = new List<BaseNode>();

        public NodeManager(MovementGraphView view, SerializedPropertyMovementLayer layer) {
            _view = view;
            StatesProperty = layer.Property.FindPropertyRelative("_states");
        }

        public void LoadExistingNodes() {
            for (int i = 0; i < StatesProperty.arraySize; i++) {
                SerializedProperty property = StatesProperty.GetArrayElementAtIndex(i);
                SerializedProperty position = property.FindPropertyRelative("_position");
                State state = property.managedReferenceValue as State;
                _view.AddElement(CreateNode(position?.vector2Value, property, state));
            }

            foreach (BaseNode node in _nodes) {
                node.LoadConnections();
            }
        }

        public void UpdatePosition(BaseNode node) {
            SerializedProperty position = node.State.FindPropertyRelative("_position");
            position.vector2Value = node.GetPosition().position;
            StatesProperty.serializedObject.ApplyModifiedProperties();
        }

        
        private BaseNode CreateNode(Vector2? position, SerializedProperty property, State state) {
            BaseNode node = state switch {
                LimitedEventState limitedEventState => new LimitedEventNode(_view, property, limitedEventState),
                EventState eventState => new EventNode(_view, property, eventState),
                RedirectState redirectState => new RedirectNode(_view, property, redirectState),
                MovementState movementState => new BoundNode(_view, property, movementState),
                _ => null
            };
            if (node == null) return null;
            if(position.HasValue) node.SetPosition(new Rect(position.Value, Vector2.zero));
            _nodes.Add(node);
            return node;
        }

        public IManipulator CreateNodeContextualMenu() {
            return new ContextualMenuManipulator(
                menuEvent => {
                    menuEvent.menu.AppendAction("Basic Nodes/Event Node", 
                        action => _view.AddElement(CreateNode<EventState>(action)));
                    menuEvent.menu.AppendAction("Basic Nodes/Limited Event Node", 
                        action => _view.AddElement(CreateNode<LimitedEventState>(action)));
                    menuEvent.menu.AppendAction("Basic Nodes/Redirect Node", action => {
                        _view.AddElement(CreateNode<RedirectState>(action));
                    });
                    
                    IEnumerable<Type> enumerable = TypeCache.GetTypesDerivedFrom(typeof(MovementState)).Where(p =>
                        (p.IsPublic || p.IsNestedPublic) && !p.IsAbstract && !p.IsGenericType &&
                        !typeof(UnityEngine.Object).IsAssignableFrom(p) &&
                        Attribute.IsDefined(p,typeof(SerializableAttribute))
                    );
                    foreach (Type type in enumerable) {
                        menuEvent.menu.AppendAction($"Movement States/{NamedState.GetName(type)}", 
                            action => _view.AddElement(CreateNode(action, type)));
                    }
                });
        }

        private BaseNode CreateNode<T> (DropdownMenuAction dropdownMenuAction) where T : State
            => CreateNode(dropdownMenuAction, typeof(T));
        
        private BaseNode CreateNode(DropdownMenuAction dropdownMenuAction, Type type) {
            return CreateNode(dropdownMenuAction.eventInfo.localMousePosition,
                () => (State) Activator.CreateInstance(type));
        }
        
        private BaseNode CreateNode(Vector2 position, Func<State> createInstance) {
            State instance = createInstance();
            SerializedProperty newElement = StatesProperty.AppendArrayElement(element => element.managedReferenceValue = instance);
            return CreateNode(position, newElement, instance);
        }

        public void DeleteNode(BaseNode node) {
            int index = StatesProperty.GetArrayIndex(node.State);
            StatesProperty.DeleteArrayElementAtIndex(index);
            StatesProperty.serializedObject.ApplyModifiedProperties();
            _nodes.Remove(node);
            
            // rebuild All
            //MovementSystemEditor.GetWindow().Rebuild();
        }
        
    }
}