using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    InputReader _inputReader;
    Transform _tf;
    CharacterController _controller;
    
    Vector3 _moveVector;
    Vector3 _velocity;
    float _gravity;
    
    [Header("Movement")]
    [SerializeField] float _walkSpeed = 5f;
    [SerializeField] float _jumpHeight = 5f;

    public bool IsGrounded => _controller.isGrounded;
    public bool IsJumping { get; private set; }
    
    void Awake()
    {
        GetComponents();
        Init();
    }
    
    void Update()
    {
        GetInput();
        if (_controller.isGrounded && IsJumping)
        {
            Jump();
            IsJumping = false;
        }
        PhysicUpdate();
    }

    void FixedUpdate()
    {
        Move();
        Debug.Log($"IsJumping = {IsJumping}, IsGrounded = {_controller.isGrounded}");
        Debug.Log($"_velocity = {_velocity}");
    }
    void Init()
    {
        _tf = gameObject.transform;
        _gravity = Physics.gravity.magnitude;
    }
    void GetInput()
    {
        _moveVector = new Vector3(_inputReader.MoveInput.x, 0, _inputReader.MoveInput.y);
        IsJumping = _inputReader.JumpInput;
    }
    void GetComponents()
    {
        _controller = GetComponent<CharacterController>();
        _inputReader = GetComponent<InputReader>();
    }
    void PhysicUpdate()
    {
        _velocity.y -= _gravity * Time.deltaTime;
        if (_velocity.y < 0)
        {
            _velocity.y = 0;
        }
    }
    void Move()
    {
        _velocity.x = _moveVector.x * _walkSpeed;
        _velocity.z = _moveVector.z * _walkSpeed;
        _controller.Move(_velocity * Time.deltaTime);
    }
    void Jump()
    {
        _velocity.y = Mathf.Sqrt(2 * _gravity * _jumpHeight);
    }
}
