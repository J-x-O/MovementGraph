using System;

namespace JescoDev.MovementGraph.MovementGraph.Attributes {
    
    [AttributeUsage(AttributeTargets.Class)]
    public class MovementMenuPath : Attribute {
        
        public string Path { get; private set; }

        public MovementMenuPath(string path) {
            Path = path;
        }

        
    }
}