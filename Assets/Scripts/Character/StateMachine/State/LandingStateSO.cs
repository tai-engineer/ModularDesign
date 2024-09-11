using UnityEngine;
namespace Character.StateMachine.State
{
    [CreateAssetMenu(fileName = "Landing", menuName = "Character/States/Grounded/Landing", order = 0)]
    public class LandingStateSO : GroundedStateSO
    {
        protected override bool TryGetGroundedTransition(out BaseStateSO newState)
        {
            return stateMachine.statesDict.TryGetValue("Idle", out newState);
        }
    }
}