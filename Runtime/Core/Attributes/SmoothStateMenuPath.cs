using System;

namespace JescoDev.SmoothBrainStates.Attributes {
    
    [AttributeUsage(AttributeTargets.Class)]
    public class SmoothStateMenuPath : Attribute {
        
        public string Path { get; private set; }

        public SmoothStateMenuPath(string path) {
            Path = path;
        }

        
    }
}