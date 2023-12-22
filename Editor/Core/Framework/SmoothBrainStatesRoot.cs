using JescoDev.MovementGraph.Editor.Utility;
using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public class SmoothBrainStatesRoot : TwoPaneSplitView {
        
        public readonly PathTracker Tracker;
        public readonly SerializedObject Root;
        
        public SmoothBrainStatesRoot(SerializedObject root) : base(0, 150f, TwoPaneSplitViewOrientation.Horizontal){
            Root = root;
            Tracker = new PathTracker(root);
            
            Add(new VisualElement() { name = "FixedPane" });
            Add(new VisualElement() { name = "FlexedPane" });
            styleSheets.Add(Loading.LoadStyleSheet("LayerView.uss"));
            styleSheets.Add(Loading.LoadStyleSheet("NodeStyles.uss"));
            
            flexedPane.Add(new Breadcrumbs(Tracker));
        }
        
        public void LoadExtensions() {
            
        }
        
    }
}