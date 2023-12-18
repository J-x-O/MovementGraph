namespace JescoDev.SmoothBrainStates.SubStates {
    public interface ISubContainer {
        public SmoothBrainStates StateMachine { get; }
        public string ResolvePath();
    }
}