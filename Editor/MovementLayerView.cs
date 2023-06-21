using System.Linq;
using Editor.MovementEditor.PropertyUtility;
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
        private readonly SerializedProperty _layers;
        private readonly SerializedProperty _selectionProperty;
        private LayerButton _selection;
        
        public MovementLayerView(MovementSystem link) : base(0, 150f, TwoPaneSplitViewOrientation.Horizontal) {
            _link = link;
            _linkObject = new SerializedObject(_link);
            _layers = _linkObject.FindProperty("_layer");
            _selectionProperty = _linkObject.FindProperty("_selectedLayer");
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
            for (int i = 0; i < _layers.arraySize; i++) {
                SerializedProperty layer = _layers.GetArrayElementAtIndex(i);
                LayerButton button = new LayerButton(layer, SelectLayer, RemoveLayer);
                fixedPane.Add(button);
                if(i == _selectionProperty.intValue) SelectLayer(button);
            }

            Button add = new Button(AddNewLayer);
            add.name = "LayerAdder";
            add.text = "+";
            fixedPane.Add(add);
        }

        private void AddNewLayer() {
            _layers.AppendArrayElement<SerializedPropertyMovementLayer>(property => {
                property.Identifier = "New Layer";
                property.ClearStates();
                property.ResetInOut();
            });
            RebuildLayers();
            LayerButton button = fixedPane.Children().OfType<LayerButton>().Last();
            SelectLayer(button);
        }

        private void SelectLayer(LayerButton layer) {
            _selection?.SetSelected(false);
            _selection = layer;
            _selection.SetSelected(true);
            
            int index = _layers.GetArrayIndex(layer.Layer);
            _selectionProperty.intValue = index;
            
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
            _layers.RemoveArrayElement(layer.Layer);
            _layers.serializedObject.ApplyModifiedProperties();
            RebuildLayers();
        }

        
        public static StyleSheet LoadStyleSheet(string localPath) {
            const string resourcePath = "Packages/com.j-x-o.movement-graph/EditorResources/";
            return (StyleSheet)EditorGUIUtility.Load(resourcePath + localPath);
        }
        
    }
}