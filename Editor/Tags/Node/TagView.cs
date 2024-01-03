using System;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Tags.Editor {
    public class TagView : VisualElement {
        private readonly SettingsDrawerTags _settings;

        private readonly Button _addButton;
        private SerializedProperty _array;

        public TagView(SettingsDrawerTags settings, SerializedProperty array) {
            _settings = settings;
            _settings.OnTagRenamed += RenameTag;
            _settings.OnColorChanged += ChangeColor;
            _settings.OnTagsChanged += OnTagsChanged;
            
            AddToClassList("tag-list-container");
            
            _addButton = new Button() { text = "Add" };
            _addButton.AddToClassList("tag-list-add-button");
            
            ContextualMenuManipulator test = new(AddItem);
            test.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });;
            _addButton.AddManipulator(test);
            
            styleSheets.Add(Loading.LoadStyleSheet("TagView.uss"));

            _array = array;
            RemoveDuplicates();
            Rebuild();
        }

        ~TagView() {
            _settings.OnTagRenamed -= RenameTag;
            _settings.OnColorChanged -= ChangeColor;
            _settings.OnTagsChanged -= OnTagsChanged;
        }

        private void OnTagsChanged() {
            RemoveUnknown();
            Rebuild();
        }
        
        private void RemoveUnknown() {
            _array.RemoveAll(prop => !_settings.HasTag(prop.stringValue));
        }
        
        private void RemoveDuplicates() {
            List<string> found = new List<string>();
            _array.RemoveAll(prop => {
                if (found.Contains(prop.stringValue)) return true;
                found.Add(prop.stringValue);
                return false;
            });
        }

        private IEnumerable<SerializedProperty> FindTagProperty(string tag) {
            return _array.ForEach().Where(property => property.stringValue == tag);
        }
        private IEnumerable<Label> FindTagLabel(string tag) {
             return Children().OfType<Label>().Where(label => label.text == tag);
         }

        private void RenameTag(string from, string to) {
            if (_array == null) return;
            bool changed = false;
            foreach (SerializedProperty property in FindTagProperty(from)) {
                property.stringValue = to;
                changed = true;
            }
            foreach (Label label in FindTagLabel(from)) {
                label.text = to;
            }

            if (!changed) return;
            _array.serializedObject.ApplyModifiedProperties();
        }
        
        private void ChangeColor(string tag, Color newColor) {
            foreach (Label label in FindTagLabel(tag)) {
                label.style.backgroundColor = newColor;
                label.style.color = TagColor.GetTextColor(newColor);
            }
        }
        
        
        // Refresh/recreate the list.
        public void Rebuild() {
            Clear();
       
            if (_array == null) return;
            for (int i = 0; i < _array.arraySize; i++) {
                
                SerializedProperty property = _array.GetArrayElementAtIndex(i);
                SerializedPropertyTag correspondingTag = _settings.GetTag(property.stringValue);
                
                if (correspondingTag == null) {
                    _array.DeleteArrayElementAtIndex(i);
                    i--;
                    return;
                }
                
                if (property.stringValue.Length == 0) property.stringValue = "NewTag";
                Label label = new Label(property.stringValue);
                label.style.backgroundColor = correspondingTag.Color;
                label.style.color = TagColor.GetTextColor(correspondingTag.Color);
                label.AddToClassList("tag-list-item");

                int index = i; // this is apparently needed, bruh
                label.AddManipulator(new ContextualMenuManipulator(evt => {
                    evt.menu.AppendSeparator();
                    evt.menu.AppendAction("Delete Tag", _ => RemoveItem(index));
                }));
                Add(label);
            }
            
            if(ToBeAdded().Any()) Add(_addButton);
        }

        // Remove an item and refresh the list
        public void RemoveItem(int index) {
            if (_array == null) return;
            _array.DeleteArrayElementAtIndex(index);
            _array.serializedObject.ApplyModifiedProperties();
            Rebuild();
        }

        // Add an item and refresh the list
        public void AddItem(ContextualMenuPopulateEvent evt) {
            if (_array == null) return;
            evt.menu.MenuItems().Clear();
            foreach (SerializedPropertyTag tag in ToBeAdded()) {
                evt.menu.AppendAction(tag.Name, _ => {
                    _array.InsertArrayElementAtIndex(_array.arraySize);
                    SerializedProperty property = _array.GetArrayElementAtIndex(_array.arraySize - 1);
                    property.stringValue = tag.Name;
                    _array.serializedObject.ApplyModifiedProperties();
                    Rebuild();
                });
            }
            evt.StopPropagation();
        }
        
        private IEnumerable<SerializedPropertyTag> ToBeAdded() {
            IEnumerable<string> existingTags = _array.ForEach().Select(property => property.stringValue);
            return _settings.Tags.Where(tag => !existingTags.Contains(tag.Name));
        }
    }
}