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
        
        [NonSerialized] public Vector3 moveVector;
        
        #region Properties
        public bool IsGrounded { get; private set; }
        public Vector2 MoveInput { get; private set; }
        public bool JumpInput { get; private set; }

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

        void FixedUpdate()
        {
            _rb.MovePosition(transform.position + moveVector * Time.deltaTime);
        }

        void Update()
        {
            GroundCheck();
            Debug.Log($"_moveVector: {moveVector}, IsGrounded: {IsGrounded}");
        }

        void GroundCheck()
        {
            IsGrounded = _groundDetector.Check();
        }
        #region Callback

        void OnMove(Vector2 moveInput) => MoveInput = moveInput;
        void OnJump(bool jumpInput) => JumpInput = jumpInput;
        #endregion
    }
}