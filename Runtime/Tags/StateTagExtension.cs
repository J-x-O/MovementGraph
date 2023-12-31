using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.SmoothBrainStates.States;
using JescoDev.Utility.EventUtility.EventUtility;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Movement.Tags {
    public class StateTagExtension : ISmoothExtension, IReadOnlyVarBasedEventSystem<string, ExecutableState> {
        
        private readonly VarBasedEventSystem<string, ExecutableState> _events = new();
#if UNITY_EDITOR
        [Serializable]
        private struct Tag {
            public string Name;
            public Color Color;
        }
        [SerializeField] private List<Tag> _tags = new();
#endif
        
        private SmoothBrainStateMashine _root;
        
        // explicit implementation, to avoid missuse
        void ISmoothExtension.Setup(SmoothBrainStateMashine root) {
            _root = root;
            root.Events.OnAnyStateActivated += InvokeStart;
        }

        void ISmoothExtension.Cleanup(SmoothBrainStateMashine root) {
            root.Events.OnAnyStateActivated -= InvokeStart;
        }
        
        private void InvokeStart(ExecutableState newState) {
            IEnumerable<string> oldTags = _root.PreviousState is ITaggedState oldTagged ? oldTagged.Tags : Array.Empty<string>();
            IEnumerable<string> newTags = newState is ITaggedState newTagged ? newTagged.Tags : Array.Empty<string>();
            
            IEnumerable<string> started = newTags.Except(oldTags);
            foreach (string tag in started) _events.InvokeVarBasedEventStart(tag, newState);

            IEnumerable<string> ended = oldTags.Except(newTags);
            foreach (string tag in ended) _events.InvokeVarBasedEventEnd(tag, newState);
        }

        public void RegisterHandler(ITypeBasedEventHandler<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _events.RegisterHandler(eventHandler, subscribedClasses);
        }

        public void RegisterStartHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _events.RegisterStartHandler(eventHandler, subscribedClasses);
        }

        public void RegisterStopHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _events.RegisterStopHandler(eventHandler, subscribedClasses);
        }

        public void UnregisterHandler(ITypeBasedEventHandler<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _events.UnregisterHandler(eventHandler, subscribedClasses);
        }

        public void UnregisterStartHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _events.UnregisterStartHandler(eventHandler, subscribedClasses);
        }

        public void UnregisterStopHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _events.UnregisterStopHandler(eventHandler, subscribedClasses);
        }
    }
}