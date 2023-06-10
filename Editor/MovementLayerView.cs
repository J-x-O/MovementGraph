using Entities.Movement;
using JescoDev.MovementGraph;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class MovementLayerView : TwoPaneSplitView {

        private readonly MovementSystem _link;
        private readonly SerializedObject _linkObject;
        private readonly SerializedProperty _layer;
        private LayerButton _selection;
        
        public MovementLayerView(MovementSystem link) : base(0, 150f, TwoPaneSplitViewOrientation.Horizontal) {
            _link = link;
            _linkObject = new SerializedObject(_link);
            _layer = _linkObject.FindProperty("_layer");
            Add(new VisualElement());
            Add(new VisualElement());
            RegisterCallback(new EventCallback<GeometryChangedEvent>(RebuildLayers));
            styleSheets.Add(LoadStyleSheet("LayerView.uss"));
            styleSheets.Add(LoadStyleSheet("NodeStyles.uss"));
        }

        private void RebuildLayers(GeometryChangedEvent evt) {
            RebuildLayers();
            UnregisterCallback<GeometryChangedEvent>(RebuildLayers);
        }

        public void RebuildLayers() {
            fixedPane.Clear();
            for (int i = 0; i < _layer.arraySize; i++) {
                SerializedProperty layer = _layer.GetArrayElementAtIndex(i);
                fixedPane.Add(new LayerButton(layer, SelectLayer, RemoveLayer));
            }

            Button add = new Button(AddNewLayer);
            add.name = "LayerAdder";
            add.text = "+";
            fixedPane.Add(add);
        }

        private void AddNewLayer() {
            _layer.AppendArrayElement(property => {
                property.FindPropertyRelative("_identifier").stringValue = "New Layer";
                property.FindPropertyRelative("_states").arraySize = 0;
            });
            RebuildLayers();
        }

        private void SelectLayer(LayerButton layer) {
            _selection?.SetSelected(false);
            _selection = layer;
            _selection.SetSelected(true);
            
            flexedPane.Clear();
            flexedPane.Add(new MovementGraphHeader(layer));
            
            MovementGraphView view = new MovementGraphView(layer.Layer);
            CameraBinder binder = new CameraBinder(_linkObject);
            view.AddManipulator(binder);
            binder.RestoreCamera();
            flexedPane.Add(view);
        }
        
        private void RemoveLayer(LayerButton layer) {
            if (_selection == layer) {
                _selection = null;
                flexedPane.Clear();
            }
            _layer.RemoveArrayElement(layer.Layer);
            RebuildLayers();
        }

        
        public static StyleSheet LoadStyleSheet(string localPath) {
            const string resourcePath = "Packages/com.j-x-o.movement-graph/EditorResources/";
            return (StyleSheet)EditorGUIUtility.Load(resourcePath + localPath);
        }
        
    }
}