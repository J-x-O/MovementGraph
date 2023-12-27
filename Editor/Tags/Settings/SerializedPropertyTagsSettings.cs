using System.Collections.Generic;
using JescoDev.SmoothBrainStates.Editor;
using UnityEditor;

namespace JescoDev.SmoothBrainStates.Tags.Editor {

    public class SerializedPropertyTag : SerializedPropertyExtension {

        public string Name {
            get => NameProperty.stringValue;
            set => NameProperty.stringValue = value;
        }
        public UnityEngine.Color Color {
            get => ColorProperty.colorValue;
            set => ColorProperty.colorValue = value;
        }
        public readonly SerializedProperty ColorProperty;
        public readonly SerializedProperty NameProperty;
        
        public SerializedPropertyTag(SerializedProperty property) : base(property) {
            NameProperty = property.FindPropertyRelative("Name"); 
            ColorProperty = property.FindPropertyRelative("Color"); 
        }
        public override bool IsValid() => Property != null && NameProperty != null && ColorProperty != null;
    }
    
    public class SerializedPropertyTagsSettings : SerializedPropertyExtension {
        
        public IEnumerable<SerializedPropertyTag> Tags {
            get {
                for (int i = 0; i < _tags.arraySize; i++) {
                    yield return new SerializedPropertyTag(_tags.GetArrayElementAtIndex(i));
                }
            }
        }

        public SerializedProperty TagProperties => _tags.Copy();
        private readonly SerializedProperty _tags;
        
        public SerializedPropertyTagsSettings(SerializedProperty property) : base(property) {
            _tags = property.FindPropertyRelative("_tags");
            CleanDuplicates();
            //ReadInExistingTags(property);
        }

        private void ReadInExistingTags(SerializedProperty property) {
            SerializedProperty iterator = property.serializedObject.GetIterator();
            while (iterator.Next(true)) {
                if (!iterator.displayName.Contains("tags")) continue;
                if (iterator.propertyType != SerializedPropertyType.String) continue;
                AddTag(iterator.stringValue);
            }
        }
        
        private void CleanDuplicates() {
            List<string> unique = new List<string>();
            for (int i = 0; i < _tags.arraySize; i++) {
                SerializedPropertyTag element = new SerializedPropertyTag(_tags.GetArrayElementAtIndex(i));
                if (!unique.Contains(element.Name)) {
                    unique.Add(element.Name);
                    continue;
                }
                _tags.DeleteArrayElementAtIndex(i);
                i--;
            }
        }
        
        public override bool IsValid() {
            return Property != null && _tags != null;
        }
        
        public SerializedProperty AddTag(string tag) {
            if (HasTag(tag)) return null;
            return _tags.AppendArrayElement(property => {
                SerializedPropertyTag newProp = new(property);
                newProp.Name = tag;
                newProp.Color = TagColor.GetColor(tag);
            });
        }
        
        public void RemoveTag(string tag) {
            _tags.RemoveArrayElement(property => new SerializedPropertyTag(property).Name == tag);
        }
        
        public bool HasTag(string tag) {
            return _tags.Any(property => new SerializedPropertyTag(property).Name == tag);
        }
    }
}