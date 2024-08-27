using UnityEngine;

namespace Character.StateMachine.State
{
    [CreateAssetMenu(fileName = "Descend", menuName = "Character/States/Descend", order = 0)]
    public class DescendStateSO : AirborneStateSO
    {
        [SerializeField]
        protected float maxFallSpeed = -50f;
        [SerializeField] 
        protected float maxRiseSpeed = 100f;
        [SerializeField] 
        protected float gravityMultiplier = 5f;
        float _verticalMovement;
        public override void Enter()
        {
            _verticalMovement = character.moveVector.y;
        }
        public override void Exit()
        {
            character.moveVector.y = 0;
        }

        public override void Update()
        {
            _verticalMovement += UnityEngine.Physics.gravity.y * 
                                 gravityMultiplier * 
                                 Time.deltaTime;

            _verticalMovement = Mathf.Clamp(_verticalMovement, 
                maxFallSpeed,
                maxRiseSpeed);
            character.moveVector.y = _verticalMovement;
        }
    }
}