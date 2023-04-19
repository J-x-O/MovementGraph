using System;
using UnityEngine.Scripting.APIUpdating;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/True")]
    [MovedFrom("Entities.ConditionSystem.FreeConditions")]
    public class ConditionTrue : ICondition {
        public bool Evaluate() => true;
    }
}