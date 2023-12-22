namespace JescoDev.SmoothBrainStates.SubStates {
    public interface ISubContainer {
        public SmoothBrainStateMashine StateMashineMachine { get; }
        public string ResolvePath();
    }
}