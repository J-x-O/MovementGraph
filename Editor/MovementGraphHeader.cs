using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class MovementGraphHeader : VisualElement {

        public MovementGraphHeader(LayerButton selectedLayer) {
            name = "GraphHeader";
            
            SerializedProperty layerIdentifier = selectedLayer.Layer.FindPropertyRelative("_identifier");
            TextField field = new TextField();
            field.name = "LayerIdentifier";
            field.BindProperty(layerIdentifier);
            field.RegisterValueChangedCallback(_ => selectedLayer.ReloadName());
            CreatePseudoLabel(field, "Identifier");

            Toggle toggle = new Toggle();
            toggle.name = "LayerAutoplay";
            SerializedProperty autoplay = selectedLayer.Layer.FindPropertyRelative("_autoplay");
            toggle.BindProperty(autoplay);
            CreatePseudoLabel(toggle, "Autoplay");

            SerializedProperty composition = selectedLayer.Layer.FindPropertyRelative("_composition");
            EnumField enumField = new EnumField();
            enumField.name = "LayerComposition";
            enumField.BindProperty(composition);
            CreatePseudoLabel(enumField, "Composition");
        }

        public void CreatePseudoLabel(VisualElement target, string identifier) {
            VisualElement element = new VisualElement();
            element.Add(new Label(identifier));
            element.Add(target);
            Add(element);
        }
        
    }
}