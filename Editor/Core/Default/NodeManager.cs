using System;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public partial class NodeManager {
        
        public readonly DefaultGraphView View;
        public readonly SerializedPropertyStateParent Layer;
        public readonly List<MovementEditorNode> Nodes = new List<MovementEditorNode>();

        public NodeManager(DefaultGraphView view, SerializedPropertyStateParent layer) {
            View = view;
            Layer = layer;
        }

        public void LoadExistingNodes() {
            foreach (SerializedPropertyState property in Layer.GetStates()) {
                if(property.State == null) continue;
                View.AddElement(CreateNode(property.Position, property, property.State));
            }

            foreach (MovementEditorNode node in Nodes) {
                node.LoadConnections();
            }
        }

        public void UpdatePosition(MovementEditorNode node) {
            node.State.Position = node.GetPosition().position;
            node.State.ApplyModifiedProperties();
        }

        
        private MovementEditorNode CreateNode(Vector2 position, SerializedPropertyState property, State state) {
            MovementEditorNode node = new MovementEditorNode(View, property, state);
            node.SetPosition(new Rect(position, Vector2.zero));
            Nodes.Add(node);
            return node;
        }

        public IManipulator CreateNodeContextualMenu() {
            return new ContextualMenuManipulator(
                menuEvent => {
                    IEnumerable<Type> enumerable = ReflectionUtility.GetAllInheritors<State>()
                        .WhereInstantiable().WhereSerializable();
                    foreach (Type type in enumerable) {
                        if(type.HasAttribute<SmoothStateHideMenu>()) continue;
                        string path = type.GetAttribute<SmoothStateMenuPath>()?.Path ?? type.Name;
                        menuEvent.menu.AppendAction(path, action => View.AddElement(CreateNode(action, type)));
                    }
                });
        }

        private MovementEditorNode CreateNode<T> (DropdownMenuAction dropdownMenuAction) where T : State
            => CreateNode(dropdownMenuAction, typeof(T));
        
        private MovementEditorNode CreateNode(DropdownMenuAction dropdownMenuAction, Type type) {
            return CreateNode(dropdownMenuAction.eventInfo.localMousePosition,
                () => (State) Activator.CreateInstance(type));
        }
        
        private MovementEditorNode CreateNode(Vector2 position, Func<State> createInstance) {
            State instance = createInstance();
            SerializedPropertyState newElement = Layer.AddState(instance);
            Layer.ApplyModifiedProperties();
            return CreateNode(position, newElement, instance);
        }

        public void DeleteNode(MovementEditorNode node) {
            if (Layer.RemoveState(node.State.State)) {
                Layer.ApplyModifiedProperties();
                Nodes.Remove(node);
            } 
            View.Rebuild();
        }
        
    }
}