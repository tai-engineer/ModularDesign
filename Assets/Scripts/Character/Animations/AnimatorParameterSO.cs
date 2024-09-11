using System;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "Animator Parameter", menuName = "Character/Animator Parameter", order = 0)]
    public class AnimatorParameterSO : ScriptableObject
    {
        public ParameterType type;
        [SerializeField] string _name;
        int _parameterHash;
        
        [SerializeField] bool _boolValue;
        [SerializeField] int _intValue;
        [SerializeField] float _floatValue;

        public Moment whenToRun;
        public enum ParameterType
        {
            Bool,Trigger,Float,Int
        }

        public enum Moment
        {
            OnStateEnter, OnStateExit
        }

        void OnEnable()
        {
            _parameterHash = Animator.StringToHash(_name);
        }

        public void SetParameter(Animator animator)
        {
            switch (type)
            {
                case ParameterType.Bool:
                    animator.SetBool(_parameterHash, _boolValue);
                    break;
                case ParameterType.Float:
                    animator.SetFloat(_parameterHash, _floatValue);
                    break;
                case ParameterType.Int:
                    animator.SetInteger(_parameterHash, _intValue);
                    break;
                case ParameterType.Trigger:
                    animator.SetTrigger(_parameterHash);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}