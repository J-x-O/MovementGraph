using JescoDev.MovementGraph.Editor;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    
    public class Breadcrumbs : VisualElement {
        
        private readonly PathTracker _tracker;

        public Breadcrumbs(PathTracker tracker) {
            _tracker = tracker;
            _tracker.OnPathChanged += OnPathChanged;
        }
        
        ~Breadcrumbs() {
            _tracker.OnPathChanged -= OnPathChanged;
        }

        private void OnPathChanged() {
            Clear();
            for (int index = 0; index < _tracker.Path.Count; index++) {
                PathTracker.PathElement element = _tracker.Path[index];
                int cut = index;
                Button label = new Button(() => _tracker.CutTo(cut));
                label.text = element.Identifier;
                label.AddToClassList("breadcrumb");
                Add(label);
            }
        }
    }
}