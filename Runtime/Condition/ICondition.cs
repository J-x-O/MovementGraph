namespace JescoDev.Utility.Condition {

    /// <summary>
    /// A Condition that can be evaluated usually for a certain type of target, depending on the implementation
    /// Used to abstract logic and make certain types of conditions exchangable
    /// </summary>
    public interface ICondition {

        public bool Evaluate();
    }
}