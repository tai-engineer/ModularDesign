namespace Character.StateMachine.State
{
    public abstract class GroundedStateSO : BaseStateSO
    {
        public sealed override bool TryGetTransition(out BaseStateSO newState)
        {
            // Falling from the ground
            if (!character.IsGrounded && !character.JumpInput)
            {
                return stateMachine.statesDict.TryGetValue("Descend", out newState);
            }
            if (character.JumpInput)
            {
                return stateMachine.statesDict.TryGetValue("Ascend", out newState);
            }
            
            return TryGetGroundedTransition(out newState);
        }

        protected virtual bool TryGetGroundedTransition(out BaseStateSO
            newGroundedState)
        {
            newGroundedState = null;
            return false;
        }
    }
}