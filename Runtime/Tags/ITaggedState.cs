using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.States;

namespace JescoDev.SmoothBrainStates.Movement.Tags {
    
    /// <summary>
    /// Serialized backing field needs to be called "_tags" for the Editor to pick it up
    /// </summary> 
    public interface ITaggedState {
        
        public IReadOnlyList<string> Tags { get; }

        public bool HasTag(string tag) => Tags.Contains(tag);
        
    }

    public static class TaggedStateExtension {
        
        public static bool HasTag(this State state, string tag) {
            return state is ITaggedState taggedState && taggedState.HasTag(tag);
        }
        
        public static bool HasTags(this State state, params string[] tags) {
            return state is ITaggedState taggedState && tags.All(taggedState.HasTag);
        }
    }
}