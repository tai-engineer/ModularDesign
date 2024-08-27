using UnityEngine;

namespace Character.StateMachine.State
{
    [CreateAssetMenu(fileName = "Run", menuName = "Character/States/Grounded/Run")]
    public class RunStateSO : GroundedStateSO
    {
        [SerializeField]
        float _runSpeed = 5f;
        public override void Update()
        {
            var movement = _runSpeed * character.MoveInput;
            character.moveVector = new Vector3(movement.x, character.moveVector.y, movement.y);
        }
        protected override bool TryGetGroundedTransition(out BaseStateSO newState)
        {
            newState = null;
            if (character.MoveInput.sqrMagnitude < 1)
            {
                return stateMachine.statesDict.TryGetValue("Idle", out newState);
            }
            return false;
        }
    }
}