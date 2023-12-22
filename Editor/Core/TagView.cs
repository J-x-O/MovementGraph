using System;
using JescoDev.MovementGraph.Editor.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JescoDev.MovementGraph.Editor {
    public class TagView : BindableElement {
        
        private readonly Button _addButton;
        private SerializedProperty _array;

        public TagView() {
            AddToClassList("tag-list-container");
            _addButton = new Button(AddItem) { text = "Add" };
            _addButton.AddToClassList("tag-list-add-button");
            
            styleSheets.Add(Loading.LoadStyleSheet("TagView.uss"));
        }
        
        // Get the reference to the bound serialized object.
        public override void HandleEvent(EventBase evt) {
            
            Type type = evt.GetType(); //SerializedObjectBindEvent is internal, so need to use reflection here
            if (type.Name == "SerializedPropertyBindEvent" && !string.IsNullOrWhiteSpace(bindingPath)) {
                SerializedProperty obj = type.GetProperty("bindProperty")?.GetValue(evt) as SerializedProperty;
                _array = obj;
                // Updating it twice here doesn't cause an issue.
                UpdateList();
            }
            base.HandleEvent(evt);
        }
        
        
        // Refresh/recreate the list.
        public void UpdateList() {
            Clear();
       
            if (_array == null) return;
            for (int i = 0; i < _array.arraySize; i++) {
                int index = i; // this is apparently needed, bruh
                
                SerializedProperty property = _array.GetArrayElementAtIndex(index);
                if (property.stringValue.Length == 0) property.stringValue = "NewTag";
                TextField propertyField = new TextField("");
                propertyField.RegisterCallback<ChangeEvent<string>>(test => {
                    if (test.newValue.Length == 0) propertyField.value = test.previousValue;
                });
                propertyField.BindProperty(property);
                propertyField.AddToClassList("tag-list-item");

                propertyField.AddManipulator(new ContextualMenuManipulator(evt => {
                    evt.menu.AppendSeparator();
                    evt.menu.AppendAction("Delete Tag", _ => RemoveItem(index));
                }));
                Add(propertyField);
            }
            
            Add(_addButton);
        }

        // Remove an item and refresh the list
        public void RemoveItem(int index) {
            if (_array == null) return;
            _array.DeleteArrayElementAtIndex(index);
            _array.serializedObject.ApplyModifiedProperties();
            UpdateList();
        }

        // Add an item and refresh the list
        public void AddItem() {
            if (_array == null) return;
            _array.InsertArrayElementAtIndex(_array.arraySize);
            _array.serializedObject.ApplyModifiedProperties();
            UpdateList();
        }
        
        public new class UxmlFactory : UxmlFactory<TagView, UxmlTraits> { }
        
        public new class UxmlTraits : BindableElement.UxmlTraits { }

    }
}