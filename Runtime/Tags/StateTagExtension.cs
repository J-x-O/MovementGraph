using System;
using System.Collections.Generic;
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

        // explicit implementation, to avoid missuse
        void ISmoothExtension.Setup(SmoothBrainStateMashine root) {
            root.Events.OnAnyStateActivated += InvokeStart;
            root.Events.OnAnyStateDeactivated += InvokeEnd;
        }

        void ISmoothExtension.Cleanup(SmoothBrainStateMashine root) {
            root.Events.OnAnyStateActivated -= InvokeStart;
            root.Events.OnAnyStateDeactivated -= InvokeEnd;
        }
        
        private void InvokeStart(ExecutableState newState) {
            if (newState is not ITaggedState tagged) return;
            foreach (string tag in tagged.Tags) {
                _events.InvokeVarBasedEventStart(tag, newState);
            }
        }

        private void InvokeEnd(ExecutableState oldState) {
            if (oldState is not ITaggedState tagged) return;
            foreach (string tag in tagged.Tags) {
                _events.InvokeVarBasedEventEnd(tag, oldState);
            }
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