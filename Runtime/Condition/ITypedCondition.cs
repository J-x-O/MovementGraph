namespace JescoDev.Utility.Condition {
    public interface ITypedCondition<T> {
        
        public bool Evaluate(T target);
        
    }
}