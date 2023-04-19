using System;
using UnityEngine;

namespace JescoDev.Utility.Condition {
    
    [Serializable]
    public abstract class TargetCondition<T> : ICondition {

        [SerializeField] private T _target;
        
        public bool Evaluate() => Evaluate(_target);

        protected abstract bool Evaluate(T target);
    }
}