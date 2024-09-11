using UnityEngine;
namespace Character.StateMachine.State
{
    [CreateAssetMenu(fileName = "Idle", menuName = "Character/States/Grounded/Idle", order = 0)]
    public class IdleStateSO : GroundedStateSO
    {
        public override void Enter()
        {
            character.moveVector.x = 0;
            character.moveVector.z = 0;
        }

        protected override bool TryGetGroundedTransition(out BaseStateSO newState)
        {
            newState = null;
            if (character.GettingMoveInput)
            {
                return stateMachine.statesDict.TryGetValue("Run", out newState);
            }

            return false;
        }
    }
}