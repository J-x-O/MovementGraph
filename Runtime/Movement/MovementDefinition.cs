
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Movement {

    public enum MovementContext { Global, Local, Teleport }
    
    public struct MovementDefinition {
        
        public Vector3 Movement;
        public MovementContext Context;

        public static MovementDefinition None => new MovementDefinition() {
            Movement = Vector3.zero,
            Context = MovementContext.Local
        };
        
        public static MovementDefinition Global(Vector3 movement) {
            return new MovementDefinition {
                Movement = movement,
                Context = MovementContext.Global
            };
        }
        
        public static MovementDefinition Local(Vector3 movement) {
            return new MovementDefinition {
                Movement = movement,
                Context = MovementContext.Local
            };
        }
        
        public static MovementDefinition Teleport(Vector3 movement) {
            return new MovementDefinition {
                Movement = movement,
                Context = MovementContext.Teleport
            };
        }

    }
}