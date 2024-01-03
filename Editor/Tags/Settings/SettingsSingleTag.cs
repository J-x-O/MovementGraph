using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Tags.Editor {

    public class TagRemovedEvent : EventBase<TagRemovedEvent> { }
    
    public class SettingsSingleTag : VisualElement {

        private string _name;
        
        public SettingsSingleTag(SerializedPropertyTag property, SettingsDrawerTags drawer) {
            name = "TagElement";
            
            TextField text = new TextField("");
            VisualElement actualText = text.Children().First();
            text.BindProperty(property.NameProperty);
            text.RegisterCallback<FocusEvent>(focus => {
                actualText.style.color = Color.white;
                _name = text.value;
            });
            text.RegisterCallback<BlurEvent>(blur => {
                actualText.style.color = TagColor.GetTextColor(property.Color);

                if (text.value.Length == 0 || drawer.CountTag(text.value) >= 2) {
                    text.SetValueWithoutNotify(_name);
                    property.Name = _name;
                    return;
                }
                
                using ChangeEvent<string> evt = ChangeEvent<string>.GetPooled(_name, text.value);
                SendEvent(evt);
            });
            Add(text);
            
            ColorField color = new ColorField();
            color.showEyeDropper = false;
            color.BindProperty(property.ColorProperty);
            color.RegisterCallback<ChangeEvent<Color>>(test => ApplyColor(actualText, test.newValue));
            ApplyColor(actualText, property.Color);
            Add(color);
            
            Add(new Button(() => {
                bool forReal = EditorUtility.DisplayDialog($"Remove Tag \"{text.value}\"?",
                    "This action cannot be undone.",
                    "Yes", "Fuck Go Back");
                if(!forReal) return;
                using TagRemovedEvent evt = TagRemovedEvent.GetPooled();
                evt.target = this;
                SendEvent(evt);
            }) { text = "-" });
        }

        private void ApplyColor(VisualElement field, Color color) {
            using ChangeEvent<Color> evt = ChangeEvent<Color>.GetPooled(field.resolvedStyle.color, color);
            style.backgroundColor = color;
            field.style.color =TagColor.GetTextColor(color);
            SendEvent(evt);
        }
        
    }
}