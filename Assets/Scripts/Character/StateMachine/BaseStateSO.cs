using Character.Physics;
using UnityEngine;
namespace Character.StateMachine
{
    public abstract class BaseStateSO : ScriptableObject, IState
    {
        public string Name;
        protected StateMachine stateMachine;
        protected CharacterPhysics character;
        protected Animator animator;
        public void AddComponents(StateMachine sm, CharacterPhysics cha, 
            Animator anim)
        {
            stateMachine = sm;
            character = cha;
            animator = anim;
        }
        public abstract bool TryGetTransition(out BaseStateSO newState);
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}