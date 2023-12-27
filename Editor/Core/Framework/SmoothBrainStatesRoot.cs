using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public class SmoothBrainStatesRoot : TwoPaneSplitView {
        
        public readonly PathTracker Tracker;
        public readonly SerializedObject Root;
        public IReadOnlyList<ISettingsDrawer> Settings => _settings;
        private List<ISettingsDrawer> _settings;
        private List<ISmoothInspector> _inspectors;
        private VisualElement _displayRoot;
        
        public SmoothBrainStatesRoot(SerializedObject root) : base(0, 150f, TwoPaneSplitViewOrientation.Horizontal){
            Root = root;
            
            InitFlex();
            
            Tracker = new PathTracker(root);
            Tracker.OnPathChanged += RefreshView;
            RegisterCallback<GeometryChangedEvent>(LoadChildren);
        }

        private void LoadChildren(GeometryChangedEvent evt) {
            UnregisterCallback<GeometryChangedEvent>(LoadChildren);
            flexedPane.Add(new Breadcrumbs(Tracker));
            _displayRoot = new VisualElement() { name = "DisplayRoot" };
            flexedPane.Add(_displayRoot);
            LoadExtensions();
            RefreshView();
        }

        ~SmoothBrainStatesRoot() {
            Tracker.OnPathChanged -= RefreshView;
        }

        private void InitFlex() {
            Add(new VisualElement() { name = "FixedPane" });
            Add(new VisualElement() { name = "FlexedPane" });
            styleSheets.Add(Loading.LoadStyleSheet("LayerView.uss"));
            styleSheets.Add(Loading.LoadStyleSheet("NodeStyles.uss"));
        }
        
        public void RefreshView() {
            _displayRoot.Clear();
            foreach (ISmoothInspector handler in _inspectors) {
                if (!handler.IsValid(Tracker.CurrentProperty)) continue;
                
                VisualElement display = handler.CreateDisplay(this, Tracker.CurrentProperty);
                if (display == null) continue;
                _displayRoot.Add(display);
                break;
            }
        }

        public T GetExtension<T>() where T : class, ISettingsDrawer {
            return Settings.FirstOrDefault(extension => extension is T) as T;
        }
        
        private void LoadExtensions() {
            _settings = ReflectionUtility.InstantiateInheritors<ISettingsDrawer>().ToList();
            _inspectors = ReflectionUtility.InstantiateInheritors<ISmoothInspector>().ToList();

            
            foreach (ISettingsDrawer handler in _settings) {
                if (!handler.IsValid(Root)) continue;
                
                Foldout foldout = new Foldout();
                foldout.name = handler.GetName();
                foldout.text = foldout.name;
                foldout.value = true;
                VisualElement settings = handler.CreateSettings(this, Root);
                if (settings == null) continue;
                foldout.contentContainer.Add(settings);
                fixedPane.Add(foldout);
            }
        }
        
    }
}