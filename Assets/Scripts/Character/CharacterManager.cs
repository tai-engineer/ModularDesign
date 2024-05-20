using UnityEngine;

namespace Character
{
    /// <summary>
    /// Manage read-only stats of character
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] InputReader _playerInput;

        #region Properties
        
        public bool IsGrounded { get; private set; }

        public bool IsJumping { get; private set; }
        public bool JumpInput => _playerInput.JumpInput;
        public Vector3 MoveInput => new Vector3(_playerInput.MoveInput.x, 0, _playerInput.MoveInput.y);

        #endregion
        
        void Awake()
        {
            GetComponents();
        }
        void GetComponents()
        {
            
        }
    }
}