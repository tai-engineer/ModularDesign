using System;
using Physics;
using UnityEngine;

namespace Character.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundDetector))]
    public class CharacterPhysics : MonoBehaviour
    {
        [SerializeField] InputReaderSO _input;

        [SerializeField]
        Transform _gameplayCamera;
        [NonSerialized] public Vector3 moveVector;
        Vector2 _inputVector;
        
        #region Properties
        public bool IsGrounded { get; private set; }
        public Vector3 MoveInput { get; private set; }
        public bool JumpInput { get; private set; }

        #endregion
        
        Rigidbody _rb;
        GroundDetector _groundDetector;
        void OnEnable()
        {
            _input.moveEvent += OnMove;
            _input.jumpStartedEvent += OnJump;
        }

        void OnDisable()
        {
            _input.moveEvent -= OnMove;
            _input.jumpStartedEvent -= OnJump;
        }

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _groundDetector = GetComponent<GroundDetector>();
        }

        void FixedUpdate()
        {
            _rb.MovePosition(transform.position + moveVector * Time.deltaTime);
        }

        void Update()
        {
            GroundCheck();
            RecalculateMovementDirection();
            Debug.Log($"_moveVector: {moveVector}, IsGrounded: {IsGrounded}");
        }

        void RecalculateMovementDirection()
        {
            Vector3 adjustedMovement;
            if (_gameplayCamera)
            {
                var cameraRight = _gameplayCamera.right;
                cameraRight.y = 0;
                var cameraForward = _gameplayCamera.forward;
                cameraForward.y = 0;

                adjustedMovement = cameraRight * _inputVector.x + cameraForward
                    * _inputVector.y;
            }
            else
            {
                adjustedMovement = new Vector3(_inputVector.x, 0, _inputVector.y);
            }

            MoveInput = adjustedMovement;
        }
        void GroundCheck()
        {
            IsGrounded = _groundDetector.Check();
        }
        #region Callback

        void OnMove(Vector2 moveInput) => _inputVector = moveInput;
        void OnJump(bool jumpInput) => JumpInput = jumpInput;
        #endregion
    }
}