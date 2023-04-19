using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/All")]
    [MovedFrom("Entities.ConditionSystem")]
    public class ConditionAll : ICondition {

        [SubclassSelector] [SerializeReference]
        private ICondition[] _conditions;

        public ConditionAll() {}
        public ConditionAll(params ICondition[] condition) => _conditions = condition;

        public bool Evaluate() {
            return _conditions.All(condition => condition.Evaluate());
        }
    }
}