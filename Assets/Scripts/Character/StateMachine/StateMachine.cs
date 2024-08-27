using System;
using System.Collections.Generic;
using Character.Physics;
using UnityEngine;

namespace Character.StateMachine
{
    [RequireComponent(typeof(CharacterPhysics))]
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        BaseStateSO _defaultState;
        [SerializeField]
        List<BaseStateSO> _states;
        
        BaseStateSO _currentState;
        [NonSerialized]
        public readonly Dictionary<string, BaseStateSO> statesDict = new 
            Dictionary<string, BaseStateSO>();
        
        CharacterPhysics _character;
        Animator _animator;
        void Awake()
        {
            _character = GetComponent<CharacterPhysics>();
            _animator = _character.GetComponent<Animator>();
        }

        void Start()
        {
            Init();
            _currentState = _defaultState;
            _currentState.Enter();
        }

        void Update()
        {
            if(_currentState.TryGetTransition(out var newState))
            {
                TransitionTo(newState);
            }
            _currentState.Update();
        }

        void Init()
        {
            if (!_states.Contains(_defaultState))
            {
                _states.Add(_defaultState);
            }

            if (_states.Count < 1)
            {
                Debug.Log("There is 0 item in States list");
                return;
            }
            
            foreach (var state in _states)
            {
                state.AddComponents(this, _character, _animator);
                statesDict.Add(state.Name, state);
            }
        }
        void TransitionTo(BaseStateSO newBaseState)
        {
            _currentState.Exit();
            _currentState = newBaseState;
            _currentState.Enter();
        }
    }
}