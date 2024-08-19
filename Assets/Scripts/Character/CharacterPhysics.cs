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
        
        Vector3 _moveVector;
        Vector2 _moveInput;
        bool _jumpInput;
        float _verticalMovement;

        [SerializeField]
        float _moveSpeed = 5f;
        float _gravity; // Gravity has negative value
        
        [SerializeField]
        float _jumpForce = 5f;

        // [SerializeField]
        // float _maxFallSpeed = -50f;
        //
        // [SerializeField]
        // float _maxRiseSpeed = 100f;
        
        public float gravityMultiplier = 5f;

        #region Properties
        public bool IsGrounded { get; private set; }

        #endregion
        
        Rigidbody _rb;
        GroundDetector _groundDetector;
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
            _groundDetector = GetComponent<GroundDetector>();
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
            IsGrounded = _groundDetector.Check();
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