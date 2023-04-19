using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/Any")]
    [MovedFrom("Entities.ConditionSystem")]
    public class ConditionAny : ICondition {

        [SubclassSelector] [SerializeReference]
        private ICondition[] _conditions;

        public ConditionAny() {}
        public ConditionAny(params ICondition[] condition) => _conditions = condition;

        public bool Evaluate() {
            return _conditions.Any(condition => condition.Evaluate());
        }
    }
}