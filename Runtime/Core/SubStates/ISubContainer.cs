namespace JescoDev.SmoothBrainStates.SubStates {
    public interface ISubContainer {
        public SmoothBrainStateMashine StateMachine { get; }
        public string ResolvePath();
    }
}