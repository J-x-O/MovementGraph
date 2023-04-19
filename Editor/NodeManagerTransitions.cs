using System.Collections.Generic;
using Entities.Movement.States;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.MovementEditor {
    public partial class NodeManager {

        private void LoadConnections() {
            foreach (BaseNode node in _nodes) {
                List<State> fuckedUp = new List<State>();
                
                foreach (State transition in node.StateObject.Transitions) {
                    BaseNode target = _nodes.Find(element => element.StateObject == transition);
                    if (target == null) {
                        Debug.LogError("Target wasn't found");
                        fuckedUp.Add(transition);
                        continue;
                    }
                    Port start = FindPort(node, Direction.Output);
                    Port end = FindPort(target, Direction.Input);
                    if (start == null || end == null) {
                        Debug.LogError("Couldn't create Edge");
                        fuckedUp.Add(transition);
                        continue;
                    }
                    _view.AddElement(start.ConnectTo(end));
                }

                foreach (State transition in fuckedUp) {
                    RemoveConnection(node, transition);
                }
            }
        }

        private static Port FindPort(Node node, Direction target) {
            return node switch {
                IConnectOut outNode when target == Direction.Output => outNode.OutputPort,
                IConnectIn inNode when target == Direction.Input => inNode.InputPort,
                _ => null
            };
        }
        
        public static void CreateConnection(BaseNode start, BaseNode end) {
            SerializedProperty property = start.State.FindPropertyRelative("_transitions");
            if(property == null) return;
            if(IsConnected(start, end)) return;
            AppendArrayElement(property, element => {
                SerializedProperty target = element.FindPropertyRelative("_target");
                target.managedReferenceValue = end.StateObject;
            });
        }

        private static bool IsConnected(BaseNode start, BaseNode end, int recursionLevel = 0) {
            
            // we found it return
            if (start == end) return true;
            
            // exit conditions
            const int recursionLimit = 50;
            if (recursionLevel > recursionLimit) return false;
            if (start is not IConnectOut connectOut) return false; // dead end

            // check every edge
            foreach (Edge outputPortConnection in connectOut.OutputPort.connections) {
                if (outputPortConnection.input.node is not BaseNode newStart) continue;
                if (IsConnected(newStart, end, recursionLevel + 1)) return true;
            }

            return false;
        }

        public static void RemoveConnection(BaseNode start, BaseNode end)
            => RemoveConnection(start, end.StateObject);
        
        public static void RemoveConnection(BaseNode start, State end) {
            SerializedProperty property = start.State.FindPropertyRelative("_transitions");
            if(property == null) return;
            for (int i = 0; i < property.arraySize; i++) {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                SerializedProperty target = element.FindPropertyRelative("_target");
                if (target.managedReferenceValue != end) continue;
                property.DeleteArrayElementAtIndex(i);
                property.serializedObject.ApplyModifiedProperties();
                return;
            }
        }
        
    }
}