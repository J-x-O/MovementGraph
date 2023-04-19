using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.MovementGraph.Editor {
    public class CameraBinder : Manipulator {

        private readonly SerializedProperty _position;
        private readonly SerializedProperty _zoom;

        public CameraBinder(SerializedObject target) {
            _position = target.FindProperty("_cameraPosition");
            _zoom = target.FindProperty("_cameraZoom");
        }
        
        protected override void RegisterCallbacksOnTarget() {
            target.RegisterCallback<MouseMoveEvent>(HandleMove);
            target.RegisterCallback<WheelEvent>(HandleZoom);
        }

        protected override void UnregisterCallbacksFromTarget() {
            target.UnregisterCallback<MouseMoveEvent>(HandleMove);
            target.UnregisterCallback<WheelEvent>(HandleZoom);
        }

        private void HandleMove(MouseMoveEvent evt) {
            if (target is not UnityEditor.Experimental.GraphView.GraphView graphView) return;

            _position.vector2Value = graphView.viewTransform.position;
            _position.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        
        private void HandleZoom(WheelEvent evt) {
            if (target is not UnityEditor.Experimental.GraphView.GraphView graphView) return;

            _zoom.floatValue = (graphView.viewTransform.scale.x + graphView.viewTransform.scale.y) / 2;
            _position.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary> This can only be called once the binder is added as a manipulator </summary>
        public void RestoreCamera() {
            if (target is not UnityEditor.Experimental.GraphView.GraphView graphView) return;
            graphView.viewTransform.position = _position.vector2Value;
            graphView.viewTransform.scale = new Vector3(_zoom.floatValue, _zoom.floatValue, 1f);
        }
    }
}