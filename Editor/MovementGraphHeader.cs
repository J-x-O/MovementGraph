using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class MovementGraphHeader : VisualElement {

        public MovementGraphHeader(LayerButton selectedLayer) {
            name = "GraphHeader";
            
            SerializedProperty layerIdentifier = selectedLayer.Layer.FindPropertyRelative("_identifier");
            TextField field = new TextField("Identifier");
            field.name = "LayerIdentifier";
            field.BindProperty(layerIdentifier);
            field.RegisterValueChangedCallback(_ => selectedLayer.ReloadName());
            Add(field);
            
            SerializedProperty composition = selectedLayer.Layer.FindPropertyRelative("_composition");
            EnumField enumField = new EnumField("Composition");
            enumField.name = "LayerComposition";
            enumField.BindProperty(composition);
            Add(enumField);
        }
        
    }
}