using System;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/False")]
    public class TypedConditionFalse<T> : ITypedCondition<T> {
        public bool Evaluate(T target) => false;
    }
}