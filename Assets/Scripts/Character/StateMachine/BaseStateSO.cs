using System.Collections.Generic;
using System.Linq;
using Character.Physics;
using UnityEngine;
namespace Character.StateMachine
{
    public abstract class BaseStateSO : ScriptableObject, IState
    {
        protected StateMachine stateMachine;
        protected CharacterPhysics character;
        protected Animator animator;
        
        public string Name;
        public List<AnimatorParameterSO> animatorParameters;
        public void AddComponents(StateMachine sm, CharacterPhysics cha, 
            Animator anim)
        {
            stateMachine = sm;
            character = cha;
            animator = anim;
        }

        public void OnStateEnter()
        {
            foreach (var parameter in animatorParameters.Where(parameter => parameter.whenToRun == AnimatorParameterSO.Moment.OnStateEnter))
            {
                parameter.SetParameter(animator);
            }

            Enter();
        }

        public void OnStateExit()
        {
            foreach (var parameter in animatorParameters.Where(parameter => 
                    parameter.whenToRun == AnimatorParameterSO.Moment
                    .OnStateExit))
            {
                parameter.SetParameter(animator);
            }
            Exit();
        }

        public void OnStateUpdate()
        {
            Update();
        }

        public abstract bool TryGetTransition(out BaseStateSO newState);
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}