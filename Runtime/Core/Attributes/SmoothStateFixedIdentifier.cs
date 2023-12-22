using System;

namespace JescoDev.SmoothBrainStates.Attributes {
    
    [AttributeUsage(AttributeTargets.Class)]
    public class SmoothStateFixedIdentifier : Attribute {
        
        public string Identifier { get; private set; }

        public SmoothStateFixedIdentifier(string identifier) {
            Identifier = identifier;
        }

    }
}