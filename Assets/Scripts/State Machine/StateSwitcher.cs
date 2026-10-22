namespace State_Machine
{
    public class StateSwitcher
    {
        public IState CurrentState { get; private set; }

        public StateSwitcher(IState startingState)
        {
            CurrentState = startingState;
        }

        public void SwitchState(IState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
