using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Manage character physics, collisions
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterManager))]
    public class CharacterPhysics : MonoBehaviour
    {
    #region Serialized Fields
        
        [Header("Movement")] 
        [SerializeField] float _walkSpeed = 5f;

        [SerializeField] float _jumpForce = 10f;
        [SerializeField] float _fallSpeed = 2f;

        [Header("Ground Collision")] 
        [SerializeField] float _sphereDistance = 0.1f;
        [SerializeField] float _sphereRadius = 1f;
        [SerializeField] LayerMask _groundLayer;
        
    #endregion

    #region Fields

        Transform _tf;
        CharacterManager _character;
        Rigidbody _rb;
        float _gravity;

        Vector3 _moveVector;
    #endregion

    #region Properties

        public bool Grounded { get; private set; }
        public Vector3 MoveVector => _moveVector;

    #endregion
        void Awake()
        {
            GetComponents();
            Initialize();
        }

        void Update()
        {
            GroundCheck();
            HorizontalMovementUpdate();
            VerticalMovementUpdate();
        }
        
        void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _moveVector * Time.fixedDeltaTime);
        }

        void GetComponents()
        {
            _character = GetComponent<CharacterManager>();
            _rb = GetComponent<Rigidbody>();
        }

        void Initialize()
        {
            _tf = transform;
            _gravity = Physics.gravity.y;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        void VerticalMovementUpdate()
        {
            if (_character.JumpInput && Grounded)
            {
                _moveVector.y = _jumpForce;
            }

            if (_moveVector.y <= 0 && Grounded)
            {
                _moveVector.y = 0;
                return;
            }

            _moveVector.y += _gravity * Time.deltaTime * _fallSpeed;
        }

        void HorizontalMovementUpdate()
        {
            Vector3 direction;
            if (_character.InputSpace)
            {
                var forward = _character.InputSpace.forward;
                forward.y = 0;
                forward.Normalize();
                var right = _character.InputSpace.right;
                right.y = 0;
                right.Normalize();
                direction = forward * _character.MoveInput.z + right * _character.MoveInput.x;
            }
            else
            {
                direction = _character.MoveInput;
            }

            var moveAmount = direction * _walkSpeed;
            _moveVector = new Vector3(moveAmount.x, _moveVector.y, moveAmount.z);
        }

        void GroundCheck()
        {
            Vector3 dir = -_tf.up;
            if (Physics.SphereCast(_tf.position, _sphereRadius, dir, out var hit, _sphereDistance, _groundLayer))
            {
                if (hit.collider)
                {
                    Grounded = true;
                    return;
                }
            }

            Grounded = false;
        }
        
    #if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            var dir = -transform.up;
            Gizmos.DrawWireSphere(transform.position + dir * _sphereDistance, _sphereRadius);
        }
    #endif
    }
}