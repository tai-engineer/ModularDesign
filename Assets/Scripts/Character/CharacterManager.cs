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

        CharacterPhysics _characterPhysics;
        #region Properties
        
        public bool IsGrounded { get; private set; }

        public bool IsJumping { get; private set; }
        public bool JumpInput => _playerInput.JumpInput;

        /// <summary>
        /// 2D Input ( x => left/right, y => forward/backward)
        /// </summary>
        public Vector3 MoveInput => _playerInput.MoveInput;
        public Transform InputSpace => _playerInputSpace;
        
        public Vector3 MoveVector => _characterPhysics == null ? Vector3.zero : _characterPhysics.MoveVector;

        public bool GettingMoveInput => _playerInput.MoveInput.x != 0 || _playerInput.MoveInput.y != 0;
        #endregion
        
        void Awake()
        {
            GetComponents();
        }
        void GetComponents()
        {
            _characterPhysics = GetComponent<CharacterPhysics>();
        }

        public void MoveToPosition(Vector3 pos)
        {
            _characterPhysics.MoveTo(pos);
        }
    }
}