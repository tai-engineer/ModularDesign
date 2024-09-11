namespace Character.StateMachine.State
{
    public abstract class AirborneStateSO : BaseStateSO
    {
        public sealed override bool TryGetTransition(out BaseStateSO newState)
        {
            if (character.IsGrounded)
            {
                return stateMachine.statesDict.TryGetValue("Landing", out newState);
            }
            return TryGetAirborneTransition(out newState);
        }
        protected virtual bool TryGetAirborneTransition(out BaseStateSO
            newAirborneState)
        {
            newAirborneState = null;
            return false;
        }
    }
}