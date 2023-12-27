using System;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    public interface IStateParent : ISubContainer {
        
        public IEnumerable<State> States { get; }
    }
    
    public interface IStateParentWithExit : IStateParent {
        public SmoothPort RegularExit { get; }
    }
    
    public static class StateParentExtensions {
        
       public static IEnumerable<T> GetStates<T>(this IStateParent parent) where T : ExecutableState {
            return parent.States.OfType<T>()
                .Concat(parent.States.OfType<IStateParent>()
                    .SelectMany(state => state.GetStates<T>()));
        }

        public static T GetState<T>(this IStateParent parent) where T : ExecutableState
            => parent.States.OfType<T>().FirstOrDefault() ??
               parent.States.OfType<IStateParent>()
                   .Select(sub => sub.GetState<T>())
                   .FirstOrDefault(element => element != null);

        public static State GetState(this IStateParent parent, string identifier)
            => parent.States.FirstOrDefault(state => state.Identifier == identifier);

        public static State GetStateByPath(this IStateParent parent, string path) {
            if (!path.Contains("/")) return parent.GetState(path);
            
            string[] split = path.Split('/');
            string identifier = split[0];
            string rest = string.Join("/", split.Skip(1));
            IStateParent sub = parent.GetState(identifier) as IStateParent;
            if (sub == null) Debug.LogWarning($"{identifier} can not be traversed further (missing: {rest})");
            return sub?.GetStateByPath(rest);
        }
        
        internal static State GetStateByGuid(this IStateParent parent, string guid)
            => parent.States.FirstOrDefault(state => state.Guid == guid);
        
        public static bool HasState<T>(this IStateParent parent) where T : ExecutableState
            => parent.GetState<T>() != null;
        
        public static bool HasState(this IStateParent parent, string identifier)
            => parent.GetState(identifier) != null;

        public static bool TryGetState<T>(this IStateParent parent, out T movementState) where T : ExecutableState {
            movementState = parent.GetState<T>();
            return movementState != null;
        }
        
        public static bool TryGetState(this IStateParent parent, string identifier, out State state){
            state = parent.GetState(identifier);
            return state != null;
        }
        
        public static bool TryGetStateByPath(this IStateParent parent, string path, out State state){
            state = parent.GetStateByPath(path);
            return state != null;
        }
        
        public static IEnumerable<State> FlattenedStates(this IStateParent parent) {
            foreach (State state in parent.States) {
                yield return state;
                if (state is IStateParent sub) {
                    foreach (State child in sub.FlattenedStates()) yield return child;
                }
            }
        }
        
    }
}