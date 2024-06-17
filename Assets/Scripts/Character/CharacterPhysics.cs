using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterManager))]
    public class CharacterPhysics : MonoBehaviour
    {
    #region Serialized Fields
        
        [Header("Movement"), Min(1f)] 
        [SerializeField] float _maxDistance = 5f;

        [SerializeField, Min(0f)]
        float _walkSpeed = 20f;
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

        Vector3 _moveVector = Vector3.zero;
        Vector3 _targetPos = Vector3.zero;
        Vector3 _prevTargetPos = Vector3.zero;
        Vector3 _direction = Vector3.zero;
    #endregion

    #region Properties

        public bool Grounded { get; private set; }
        public Vector3 MoveVector => _moveVector;
        public Vector3 Direction => _direction;

        bool MovingToTarget { get; set; }
    #endregion
        void Awake()
        {
            GetComponents();
            Initialize();
        }

        void Update()
        {
            GroundCheck();
            Jump();
            UpdateMoveVector();
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

        void UpdateMoveVector()
        {
            float distance = GetDistance();
            _direction = GetDirection();
            Vector3 moveAmount = _direction * distance;
            
            _moveVector = Vector3.MoveTowards(_moveVector, moveAmount, Time.deltaTime * _walkSpeed);
            ApplyGravity();
        }

        void ApplyGravity()
        {
            if (_moveVector.y <= 0 && Grounded)
            {
                _moveVector.y = 0;
                return;
            }

            _moveVector.y += _gravity * Time.deltaTime * _fallSpeed;
        }

        void Jump()
        {
            if (_character.JumpInput && Grounded)
            {
                _moveVector.y = _jumpForce;
            }
        }
        public void MoveTo(Vector3 target)
        {
            MovingToTarget = true;
            _targetPos = target;
        }
        Vector3 GetDirection()
        {
            if (!_character.GettingMoveInput) return MovingToTarget ? GetDirection(_targetPos) : Vector3.zero;
            Vector3 dir = Vector3.zero;
            if (_character.InputSpace)
            {
                var forward = _character.InputSpace.forward;
                forward.y = 0;
                forward.Normalize();
                var right = _character.InputSpace.right;
                right.y = 0;
                right.Normalize();
                dir = forward * _character.MoveInput.y + right * _character.MoveInput.x;
            }
            else
            {
                dir = new Vector3(_character.MoveInput.y, 0, _character.MoveInput.x);
            }

            return dir.normalized;

        }

        Vector3 GetDirection(Vector3 target)
        {
            return (target - _tf.position).normalized;
        }

        float GetDistance()
        {
            return MovingToTarget ? GetDistance(_targetPos) : _maxDistance;
        }

        float GetDistance(Vector3 target)
        {
            float dist = Vector3.Distance(_tf.position, target);
            if (dist > 0.5f)
            {
                return dist;
            }
            
            _prevTargetPos = _targetPos;
            MovingToTarget = false;

            return 0f;
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