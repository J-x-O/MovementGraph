using UnityEditor;

namespace JescoDev.SmoothBrainStates.Editor {
    public class SerializedObjectOrProperty {
        
        // object that can be automatically casted from either SerializedObject or SerializedProperty
        private readonly object _object;
        
        public SerializedObjectOrProperty(SerializedObject serializedObject) {
            _object = serializedObject;
        }
        
        public SerializedObjectOrProperty(SerializedProperty serializedProperty) {
            _object = serializedProperty;
        }
        
        public SerializedProperty FindProperty(string propertyPath) {
            return _object switch {
                SerializedObject obj => obj.FindProperty(propertyPath),
                SerializedProperty prop => prop.FindPropertyRelative(propertyPath),
                _ => null
            };
        }

        public bool IsSerializedObject() => _object is SerializedObject;
        public bool IsSerializedProperty() => _object is SerializedProperty;
        public SerializedObject AsSerializedObject() => _object as SerializedObject;
        public SerializedProperty AsSerializedProperty() => _object as SerializedProperty;

        public static implicit operator SerializedObjectOrProperty(SerializedObject serializedObject) {
            return new SerializedObjectOrProperty(serializedObject);
        }
        
        public static implicit operator SerializedObjectOrProperty(SerializedProperty serializedProperty) {
            return new SerializedObjectOrProperty(serializedProperty);
        }
        
        
        public SerializedObject serializedObject => (_object as SerializedProperty)?.serializedObject ?? _object as SerializedObject;
        public string propertyPath => (_object as SerializedProperty)?.propertyPath ?? "";
        
    }
}