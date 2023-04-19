using System;
using System.Linq;
using UnityEngine;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/All")]
    public class TypedConditionAll<T> : ITypedCondition<T> {
        
        [SerializeReference, SubclassSelector] 
        private ITypedCondition<T>[] _conditions;
        
        public TypedConditionAll() {}
        public TypedConditionAll(params ITypedCondition<T>[] condition) => _conditions = condition;

        public bool Evaluate(T target) {
            return _conditions.All(condition => condition.Evaluate(target));
        }
    }
}