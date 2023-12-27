using JescoDev.MovementGraph.Editor;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    
    public class Breadcrumbs : VisualElement {
        
        private readonly PathTracker _tracker;

        public Breadcrumbs(PathTracker tracker) {
            _tracker = tracker;
            _tracker.OnPathChanged += OnPathChanged;
            OnPathChanged();
        }
        
        ~Breadcrumbs() {
            _tracker.OnPathChanged -= OnPathChanged;
        }

        private void OnPathChanged() {
            Clear();
            AddBreadcrumb("Root", 0);
            for (int index = 0; index < _tracker.Path.Count; index++) {
                Add(new Label("->"));
                AddBreadcrumb(_tracker.Path[index].Identifier, index + 1);
            }
        }
        
        private void AddBreadcrumb(string identifier, int index) {
            Button label = new Button(() => _tracker.CutTo(index)) {
                name = "BreadcrumbPathButton",
                text = identifier
            };
            Add(label);
        }
    }
}