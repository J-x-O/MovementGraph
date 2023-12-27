using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace JescoDev.SmoothBrainStates.Editor {
    public class PathTracker {
        
        public struct PathElement {
            public string Identifier;
            public string Path;
        }
        
        public event Action OnPathChanged;
        
        public IReadOnlyList<PathElement> Path => _path;
        private List<PathElement> _path = new();

        public readonly SerializedObject Root;
        public SerializedObjectOrProperty CurrentProperty { get; private set; }
        
        public PathTracker(SerializedObject root) {
            Root = root;
            CurrentProperty = Root;
        }

        public bool EnterArrayElement(string variableName, int index, string identifier) {
            return TryUpdatePath(new PathElement() {
                Identifier = ObjectNames.NicifyVariableName(identifier),
                Path = $".{variableName}.Array.data[{index}]"
            });
        }
        
        public bool EnterSerializedProperty(string identifier, SerializedProperty property) {
            return TryUpdatePath(new PathElement() {
                Identifier = ObjectNames.NicifyVariableName(identifier),
                Path = !string.IsNullOrEmpty(CurrentProperty.propertyPath)
                    ? property.propertyPath.Replace(CurrentProperty.propertyPath, "")
                    : property.propertyPath
            });
        }
        
        public bool EnterProperty(string identifier) {
            return TryUpdatePath(new PathElement() {
                Identifier = ObjectNames.NicifyVariableName(identifier),
                Path = $".{identifier}"
            });
        }
        
        public bool EnterMultiple(string identifier, IEnumerable<string> variables) {
            return TryUpdatePath(new PathElement() {
                Identifier = identifier,
                Path = $".{string.Join(".", variables)}"
            });
        }

        public void Leave() {
            _path.RemoveAt(_path.Count - 1);
            ShortenPath();
        }
        
        public void CutTo(int elements) {
            if (elements < 0 || _path.Count <= elements ) return;
            if (elements == 0) { Reset(); return; }
            _path.RemoveRange(elements, _path.Count - elements);
            ShortenPath();
        }
        
        public void Reset() {
            _path.Clear();
            CurrentProperty = Root;
            OnPathChanged?.Invoke();
        }
        
        private bool TryUpdatePath(PathElement element) {
            string newPath = string.Join("", _path.Select(p => p.Path).Append(element.Path));
            SerializedProperty newProperty = Root.FindProperty(newPath);
            if (newProperty == null) return false;
            _path.Add(element);
            CurrentProperty = newProperty;
            OnPathChanged?.Invoke();
            return true;
        }

        private void ShortenPath() {
            string newPath = string.Join("", _path.Select(p => p.Path));
            CurrentProperty = Root.FindProperty(newPath);
            OnPathChanged?.Invoke();
        }
    }
}