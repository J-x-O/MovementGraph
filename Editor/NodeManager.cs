using System;
using System.Collections.Generic;
using System.Linq;
using Editor.MovementEditor.PropertyUtility;
using Entities.Movement.States;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public partial class NodeManager {
        
        public readonly MovementGraphView _view;
        public readonly SerializedPropertyMovementLayer _layer;
        public readonly List<BaseNode> _nodes = new List<BaseNode>();

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

            foreach (BaseNode node in _nodes) {
                node.LoadConnections();
            }
        }

        public void UpdatePosition(BaseNode node) {
            node.State.Position = node.GetPosition().position;
            node.State.ApplyModifiedProperties();
        }

        
        private BaseNode CreateNode(Vector2 position, SerializedPropertyState property, State state) {
            BaseNode node = state switch {
                LimitedEventState limitedEventState => new LimitedEventNode(_view, property, limitedEventState),
                EventState eventState => new EventNode(_view, property, eventState),
                RedirectState redirectState => new RedirectNode(_view, property, redirectState),
                MovementState movementState => new BoundNode(_view, property, movementState),
                LayerIn inState => new LayerInNode(_view, property, inState),
                LayerOut outState => new LayerOutNode(_view, property, outState),
                _ => null
            };
            if (node == null) return null;
            node.SetPosition(new Rect(position, Vector2.zero));
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
                        menuEvent.menu.AppendAction($"Movement States/{MovementState.GetName(type)}", 
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
            SerializedPropertyState newElement = _layer.AddState(instance);
            _layer.ApplyModifiedProperties();
            return CreateNode(position, newElement, instance);
        }

        public void DeleteNode(BaseNode node) {
            if (_layer.RemoveState(node.State.State)) {
                _layer.ApplyModifiedProperties();
                _nodes.Remove(node);
            } 
            
            _view.Rebuild();
        }
        
    }
}