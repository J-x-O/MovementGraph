using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JescoDev.SmoothBrainStates.Editor;
using JescoDev.SmoothBrainStates.Movement.Tags;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Tags.Editor {
    public class SettingsDrawerTags : ISettingsDrawer {
        
        public event Action OnTagsChanged;
        public event Action<string, Color> OnColorChanged;
        public event Action<string, string> OnTagRenamed;

        public IEnumerable<SerializedPropertyTag> Tags => _property.Tags;
        private VisualElement _root;
        private SerializedPropertyTagsSettings _property;
        public bool IsValid(SerializedObject target) {
            return true;
        }

        public VisualElement CreateSettings(SmoothBrainStatesRoot root, SerializedObject target) {
            _root = new VisualElement();
            _root.name = "TagSettings";
            _root.styleSheets.Add(Loading.LoadStyleSheet("TagSettings.uss"));
            _property = new SerializedPropertyTagsSettings(ExtensionUtility.GetOrCreateExtension<StateTagExtension>(target));
            RebuildList();
            return _root;
        }

        public string GetName() => "Tag Manager";
        
        public SerializedPropertyTag GetTag(string tag) {
            return _property.Tags.FirstOrDefault(t => t.Name == tag);
        }
        
        public bool HasTag(string tag) {
            return _property.Tags.Any(t => t.Name == tag);
        }
        
        public int CountTag(string tag) {
            return _property.Tags.Count(t => t.Name == tag);
        }
        
        private void AddTag(string tag) {
            SerializedProperty property = _property.AddTag(tag);
            if (property == null) return;
            RebuildList();
            OnTagsChanged?.Invoke();
        }
        
        private void RemoveTag(string tag) {
            _property.RemoveTag(tag);
            RebuildList();
            OnTagsChanged?.Invoke();
        }
        
        private void RebuildList() {
            _root.Clear();
            foreach (SerializedPropertyTag tag in _property.Tags) {
                SettingsSingleTag tagElement = new SettingsSingleTag(tag, this);
                tagElement.RegisterCallback<TagRemovedEvent>(_ => RemoveTag(tag.Name));
                tagElement.RegisterCallback<ChangeEvent<string>>(evt => {
                    OnTagRenamed?.Invoke(evt.previousValue, evt.newValue);
                });
                tagElement.RegisterCallback<ChangeEvent<Color>>(evt => {
                    OnColorChanged?.Invoke(tag.Name, evt.newValue);
                });
                _root.Add(tagElement);
            }
            _root.Add(new Button(AddEmptyTag) { text = "Add Tag" });
        }
        
        private int FindLowestNumber() {
            List<int> numbers = Tags.Where(tag => tag.Name.StartsWith("New Tag"))
                .Select(tag => Regex.Match(tag.Name, @"\d+").Value)
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(int.Parse)
                .ToList();
            IEnumerable<int> sequence = Enumerable.Range(1, numbers.Any() ? numbers.Max() + 1 : 1);
            return sequence.Except(numbers).First();
        }

        private void AddEmptyTag() {
            AddTag($"New Tag ({FindLowestNumber()})");
        }
    }
}