using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class LayerButton : VisualElement {

        public readonly SerializedProperty Layer;
        public readonly Button SelectButton;
        
        public LayerButton(SerializedProperty layer, Action<LayerButton> select, Action<LayerButton> remove) {
            Layer = layer;
            
            name = "LayerContainer";
            SelectButton = new Button(() => select(this));
            SelectButton.name = "LayerSelector";
            ReloadName();
            Add(SelectButton);

            Button removeButton = new Button(() => remove(this));
            removeButton.name = "LayerRemover";
            removeButton.text = "-";
            Add(removeButton);
        }

        public void ReloadName() {
            SerializedProperty layerIdentifier = Layer.FindPropertyRelative("_identifier");
            SelectButton.text = $"{layerIdentifier.stringValue}";
        }

        public void SetSelected(bool state) {
            if(state) AddToClassList("selected");
            else RemoveFromClassList("selected");
        }
        
    }
}