namespace Character.StateMachine
{
    public interface IState
    {
        void OnStateEnter();
        void OnStateExit();
        void OnStateUpdate();
    }
}