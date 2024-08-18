using System;
using UnityEngine;

namespace Character.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterPhysics : MonoBehaviour
    {
        Rigidbody _rb;
        [SerializeField] InputReaderSO _input;
        
        Vector3 _moveVector = Vector3.zero;
        Vector2 _moveInput;
        bool _jumpInput;

        [SerializeField]
        float _moveSpeed = 5f;
        float _gravity; // Gravity has negative value
        public bool IsGrounded { get; private set; }

        [SerializeField]
        [Tooltip("An empty GameObject to specify the check position")]
        Transform _groundCheck;

        [SerializeField]
        [Tooltip("Radius of the sphere to check for ground")]
        float _groundCheckRadius = 0.2f;

        [SerializeField]
        [Tooltip("Layer that represents the ground")]
        LayerMask _groundLayer;

        [SerializeField]
        float _jumpForce = 5f;

        // [SerializeField]
        // float _maxFallSpeed = -50f;
        //
        // [SerializeField]
        // float _maxRiseSpeed = 100f;
        
        float _verticalMovement;
        public float gravityMultiplier = 5f;
        void OnEnable()
        {
            _input.OnMoveEvent += OnMove;
            _input.OnJumpStartEvent += OnJump;
        }

        void OnDisable()
        {
            _input.OnMoveEvent -= OnMove;
            _input.OnJumpStartEvent -= OnJump;
        }

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            _gravity = UnityEngine.Physics.gravity.y;
        }

        void FixedUpdate()
        {
            _rb.MovePosition(transform.position + _moveVector * Time.deltaTime);
        }

        void Update()
        {
            GroundCheck();
            GroundMovement();
            AirborneMovement();
            Debug.Log($"_moveVector: {_moveVector}, IsGrounded: {IsGrounded}");
        }

        void Jump()
        {
            _verticalMovement = _jumpForce;
        }
        void GroundCheck()
        {
            IsGrounded = UnityEngine.Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        }
        void AirborneMovement()
        {
            if (IsGrounded && _jumpInput)
            {
                Jump();
            }
            else if (IsGrounded)
            {
                _verticalMovement = 0;
            }
            else
            {
                _verticalMovement += _gravity * gravityMultiplier * Time.deltaTime;
                // _verticalMovement = Mathf.Clamp(_verticalMovement, _maxFallSpeed, _maxRiseSpeed);
            }

            _moveVector.y = _verticalMovement;
        }

        void GroundMovement()
        {
            
            var movement = _moveSpeed * _moveInput;
            _moveVector = new Vector3(movement.x, _moveVector.y, movement.y);
        }

        #region Callback

        void OnMove(Vector2 moveInput) => _moveInput = moveInput;
        void OnJump(bool jumpInput) => _jumpInput = jumpInput;
        #endregion
    }
}