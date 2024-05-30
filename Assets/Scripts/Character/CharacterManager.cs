using TMPro;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Manage read-only stats of character
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] InputReaderSO _playerInput;

        [SerializeField]
        Transform _playerInputSpace;

        CharacterPhysics _physics;
        #region Properties
        
        public bool IsGrounded { get; private set; }

        public bool IsJumping { get; private set; }
        public bool JumpInput => _playerInput.JumpInput;
        public Vector3 MoveInput => new Vector3(_playerInput.MoveInput.x, 0, _playerInput.MoveInput.y);
        public Transform InputSpace => _playerInputSpace;

        public Vector3 MoveVector => _physics == null ? Vector3.zero : _physics.MoveVector;

        #endregion
        
        void Awake()
        {
            GetComponents();
        }
        void GetComponents()
        {
            _physics = GetComponent<CharacterPhysics>();
        }
    }
}