using UnityEngine;

namespace Character.StateMachine.State
{
    [CreateAssetMenu(fileName = "Ascend", menuName = "Character/States/Ascend", order = 0)]
    public class AscendStateSO : AirborneStateSO
    {
        [SerializeField]
        float _jumpForce = 6f;
        [SerializeField]
        protected float gravityComebackMultiplier = .03f;
        [SerializeField] 
        protected float gravityMultiplier = 5f;
        [SerializeField]
        protected float gravityDivider = 0.6f;
        float _verticalMovement;
        float _gravityContributionMultiplier;

        public override void Enter()
        {
            _verticalMovement = _jumpForce;
        }

        public override void Update()
        {
            _gravityContributionMultiplier += gravityComebackMultiplier;
            _gravityContributionMultiplier *= gravityDivider; //Reduce gravity effect
            _verticalMovement += UnityEngine.Physics.gravity.y * 
                                 gravityMultiplier * 
                                 Time.deltaTime * _gravityContributionMultiplier;
            
            character.moveVector.y = _verticalMovement;
        }

        protected override bool TryGetAirborneTransition(out BaseStateSO newState)
        {
            newState = null;
            if (character.moveVector.y < 0)
            {
                return stateMachine.statesDict.TryGetValue("Descend", out newState);
            }

            return false;
        }
    }
}