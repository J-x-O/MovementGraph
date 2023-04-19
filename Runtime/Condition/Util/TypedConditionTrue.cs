using System;

namespace JescoDev.Utility.Condition.Util {
    
    [Serializable]
    [AddTypeMenu("Util/True")]
    public class TypedConditionTrue<T> : ITypedCondition<T> {
        public bool Evaluate(T target) => true;
    }
}