using JescoDev.MovementGraph.Layer;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class MovementGraphHeader : VisualElement {

        public MovementGraphHeader(LayerButton selectedLayer) {
            name = "GraphHeader";
            
            TextField textField = CreatePseudoElement<TextField>(selectedLayer.Layer, "LayerIdentifier", "_identifier", "Identifier");
            textField.RegisterValueChangedCallback(_ => selectedLayer.ReloadName());

            CreatePseudoElement<Toggle>(selectedLayer.Layer, "LayerAutoplay", "_autoplay", "Autoplay");
            CreatePseudoElement<Toggle>(selectedLayer.Layer, "LayerPlayIfInactive", "_playIfInactive", "Play If Inactive");
            CreatePseudoElement<EnumField>(selectedLayer.Layer, "LayerComposition", "_composition", "Composition");
        }
        
        private T CreatePseudoElement<T>(SerializedProperty layer, string identifier, string property, string label) where T : VisualElement, IBindable, new() {
            T element = new T();
            element.name = identifier;
            SerializedProperty playIfInactive = layer.FindPropertyRelative(property);
            element.BindProperty(playIfInactive);
            CreatePseudoLabel(element, label);
            return element;
        }

        private void CreatePseudoLabel(VisualElement target, string label) {
            VisualElement element = new VisualElement();
            element.Add(new Label(label));
            element.Add(target);
            Add(element);
        }
        
    }
}