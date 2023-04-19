using System;
using UnityEngine.Scripting.APIUpdating;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/False")]
    [MovedFrom("Entities.ConditionSystem.FreeConditions")]
    public class ConditionFalse : ICondition {
        public bool Evaluate() => false;
    }
}