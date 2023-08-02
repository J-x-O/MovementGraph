using System;

namespace JescoDev.MovementGraph.Editor.Attributes {
    
    [AttributeUsage(AttributeTargets.Class)]
    public class FixedStateIdentifier : Attribute {
        
        public string Identifier { get; private set; }

        public FixedStateIdentifier(string identifier) {
            Identifier = identifier;
        }

    }
}