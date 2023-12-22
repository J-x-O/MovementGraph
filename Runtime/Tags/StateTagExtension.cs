using System;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.SmoothBrainStates.States;
using JescoDev.Utility.EventUtility.EventUtility;

namespace JescoDev.SmoothBrainStates.Movement.Tags {
    public class StateTagExtension : ISmoothExtension, IReadOnlyVarBasedEventSystem<string, ExecutableState> {
        
        private readonly VarBasedEventSystem<string, ExecutableState> _tags = new();

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
                _tags.InvokeVarBasedEventStart(tag, newState);
            }
        }

        private void InvokeEnd(ExecutableState oldState) {
            if (oldState is not ITaggedState tagged) return;
            foreach (string tag in tagged.Tags) {
                _tags.InvokeVarBasedEventEnd(tag, oldState);
            }
        }
        
        public void RegisterHandler(ITypeBasedEventHandler<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _tags.RegisterHandler(eventHandler, subscribedClasses);
        }

        public void RegisterStartHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _tags.RegisterStartHandler(eventHandler, subscribedClasses);
        }

        public void RegisterStopHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _tags.RegisterStopHandler(eventHandler, subscribedClasses);
        }

        public void UnregisterHandler(ITypeBasedEventHandler<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _tags.UnregisterHandler(eventHandler, subscribedClasses);
        }

        public void UnregisterStartHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _tags.UnregisterStartHandler(eventHandler, subscribedClasses);
        }

        public void UnregisterStopHandler(Action<ExecutableState> eventHandler, params string[] subscribedClasses) {
            _tags.UnregisterStopHandler(eventHandler, subscribedClasses);
        }
    }
}