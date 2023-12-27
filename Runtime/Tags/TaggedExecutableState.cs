using System.Collections.Generic;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Movement.Tags {
    
    /// <summary> Example Implementation for the base executable state </summary>
    public abstract class TaggedExecutableState : ExecutableState, ITaggedState {
        
        public IReadOnlyList<string> Tags => _tags;
        [SerializeField] private List<string> _tags = new();

        public TaggedExecutableState() {}
        public TaggedExecutableState(string identifier) : base(identifier) { }
        
        public bool HasTag(string tag) => _tags.Contains(tag);
    }
}