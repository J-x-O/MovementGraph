using System.Collections.Generic;

namespace JescoDev.SmoothBrainStates.Movement.Tags {
    
    /// <summary>
    /// Serialized backing field needs to be called "_tags" for the Editor to pick it up
    /// </summary> 
    public interface ITaggedState {
        
        public IReadOnlyList<string> Tags { get; }

    }
}