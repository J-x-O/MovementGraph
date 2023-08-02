using System;
using System.Collections.Generic;
using System.Linq;
using Editor.MovementEditor.PropertyUtility;
using Entities.Movement.States;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.Editor.Editor.Utility;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public partial class NodeManager {
        
        public readonly MovementGraphView _view;
        public readonly SerializedPropertyMovementLayer _layer;
        public readonly List<MovementEditorNode> _nodes = new List<MovementEditorNode>();

        public NodeManager(MovementGraphView view, SerializedPropertyMovementLayer layer) {
            _view = view;
            _layer = layer;
        }

        public void LoadExistingNodes() {
            foreach (SerializedPropertyState property in _layer.GetStates()) {
                if(property.State == null) continue;
                // todo place imposter node
                _view.AddElement(CreateNode(property.Position, property, property.State));
            }

            foreach (MovementEditorNode node in _nodes) {
                node.LoadConnections();
            }
        }

        public void UpdatePosition(MovementEditorNode node) {
            node.State.Position = node.GetPosition().position;
            node.State.ApplyModifiedProperties();
        }

        
        private MovementEditorNode CreateNode(Vector2 position, SerializedPropertyState property, State state) {
            MovementEditorNode node = new MovementEditorNode(_view, property, state);
            node.SetPosition(new Rect(position, Vector2.zero));
            _nodes.Add(node);
            return node;
        }

        public IManipulator CreateNodeContextualMenu() {
            return new ContextualMenuManipulator(
                menuEvent => {
                    IEnumerable<Type> enumerable = ReflectionUtility.GetAllInheritors<State>()
                        .WhereInstantiable().WhereSerializable();
                    foreach (Type type in enumerable) {
                        if(type.HasAttribute<MovementHideMenu>()) continue;
                        string path = type.GetAttribute<MovementMenuPath>()?.Path ?? MovementState.GetName(type);
                        menuEvent.menu.AppendAction(path, action => _view.AddElement(CreateNode(action, type)));
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
            SerializedPropertyState newElement = _layer.AddState(instance);
            _layer.ApplyModifiedProperties();
            return CreateNode(position, newElement, instance);
        }

        public void DeleteNode(MovementEditorNode node) {
            if (_layer.RemoveState(node.State.State)) {
                _layer.ApplyModifiedProperties();
                _nodes.Remove(node);
            } 
            
            _view.Rebuild();
        }
        
    }
}