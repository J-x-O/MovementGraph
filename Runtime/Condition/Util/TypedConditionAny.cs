using System;
using System.Linq;
using UnityEngine;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/Any")]
    public class TypedConditionAny<T> : ITypedCondition<T> {
        
        [SubclassSelector] [SerializeReference] 
        private ITypedCondition<T>[] _conditions;
        
        public TypedConditionAny() {}
        public TypedConditionAny(params ITypedCondition<T>[] condition) => _conditions = condition;

        public bool Evaluate(T target) {
            return _conditions.Any(condition => condition.Evaluate(target));
        }
    }
}