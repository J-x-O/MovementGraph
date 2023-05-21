using Entities.Movement;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class MovementLayerView : TwoPaneSplitView {

        private readonly MovementSystem _link;
        private readonly SerializedObject _linkObject;
        private readonly SerializedProperty _layer;
        
        public MovementLayerView(MovementSystem link) {
            _link = link;
            _linkObject = new SerializedObject(_link);
            _layer = _linkObject.FindProperty("_layer");
        }

        public void RebuildLayers() {
            fixedPane.Clear();
            for (int i = 0; i < _layer.arraySize; i++) {
                SerializedProperty layer = _layer.GetArrayElementAtIndex(i);
                SerializedProperty layerIdentifier = layer.FindPropertyRelative("_identifier");
                VisualElement layerElement = new VisualElement() { name = "LayerContainer" };
                Button select = new Button(() => SelectLayer(layer));
                select.name = "LayerSelector";
                select.text = $"{layerIdentifier.stringValue}";
                layerElement.Add(select);

                Button remove = new Button(() => RemoveLayer(layer));
                remove.name = "LayerRemover";
                remove.text = "-";
                layerElement.Add(remove);
                fixedPane.Add(layerElement);
            }

            Button add = new Button(AddNewLayer);
        }

        private void AddNewLayer() {
            _layer.AppendArrayElement(property
                => property.FindPropertyRelative("_identifier").stringValue = "New Layer");
        }

        private void SelectLayer(SerializedProperty layer) {
            flexedPane.Clear();
            
            SerializedProperty layerIdentifier = layer.FindPropertyRelative("_identifier");
            TextField field = new TextField("Identifier");
            field.name = "LayerIdentifier";
            field.BindProperty(layerIdentifier);
            flexedPane.Add(field);
            
            MovementGraphView view = new MovementGraphView(layer);
            CameraBinder binder = new CameraBinder(_linkObject);
            view.AddManipulator(binder);
            binder.RestoreCamera();
            flexedPane.Add(view);
        }
        
        private void RemoveLayer(SerializedProperty layer) {
            _layer.RemoveArrayElement(layer);
            RebuildLayers();
        }

        
    }
}