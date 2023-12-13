using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerObj;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameInput _gameInput;
    //[SerializeField] private Transform _spawnPoint;

    [Header("Attributes")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private int _maxJumpCount;

    private int _gold = 0;
    private int _jumpCount;
    private float _moveSpeed = 3f;
    private float _raycastLenght = 0.2f;
    private bool _isGrounded;
    private bool _readyToJump;
    private bool _isJumping;
    private bool _isSprinting;
    private bool _exitingSlope;

    private Vector3 _moveDirection;
    private Vector2 _inputDirection;

    private RaycastHit _slopeHit;

    public static event Action<int> OnGoldCollected;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    private void Start()
    {
        _gameInput.MovementEvent += HandleMovement;
        _gameInput.SprintEvent += HandleSprint;
        _gameInput.SprintCancelledEvent += HandleSprintCancelled;
        _gameInput.JumpEvent += HandleJump;
        _gameInput.JumpCancelledEvent += HandleJumpCancelled;

        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;

        _gold = 0;

        _readyToJump = true;
    }

    private void HandleSprintCancelled()
    {
        _isSprinting = false;
    }

    private void HandleSprint()
    {
        _isSprinting = true;
    }

    private void HandleJumpCancelled()
    {
        _isJumping = false;
    }

    private void HandleJump()
    {
        _isJumping = true;
    }

    private void HandleMovement(Vector2 dir)
    {
        _inputDirection = dir;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _raycastLenght, _groundLayer);

        if (_isGrounded)
        {
            _playerRigidbody.drag = _groundDrag;
        }
        else
        {
            _playerRigidbody.drag = 0;
        }

        //CameraRotation();

        SpeedLimit();

        JumpAction();

        StateHandler();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gold"))
        {
            _gold++;
            OnGoldCollected?.Invoke(_gold);
            other.transform.position = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        }
    }

    private void StateHandler()
    {
        if (_isGrounded && _isSprinting)
        {
            state = MovementState.sprinting;
            _moveSpeed = _sprintSpeed;
        }
        else if (_isGrounded)
        {
            state = MovementState.walking;
            _moveSpeed = _walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    private void OnMove()
    {
        Vector3 convertedInputDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);
        _moveDirection = _orientation.forward * convertedInputDirection.z + _orientation.right * convertedInputDirection.x;
       
        if (OnSlope() && !_exitingSlope)
        {
            _playerRigidbody.AddForce(GetSlopeMoveDirection() * _moveSpeed * 20f, ForceMode.Force);

            if (_playerRigidbody.velocity.y > 0)
            {
                _playerRigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (_isGrounded)
        {
            _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        }
        else if (!_isGrounded)
        {
            _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
        }

        _playerRigidbody.useGravity = !OnSlope();
    }

    private void SpeedLimit()
    {
        if (OnSlope() && !_exitingSlope)
        {
            if (_playerRigidbody.velocity.magnitude > _moveSpeed)
            {
                _playerRigidbody.velocity = _playerRigidbody.velocity.normalized * _moveSpeed;
            }
        }
        else
        {
            Vector3 baseVelocity = new Vector3(_playerRigidbody.velocity.x, 0f, _playerRigidbody.velocity.z);

            if (baseVelocity.magnitude > _moveSpeed)
            {
                Vector3 limitedVelocity = baseVelocity.normalized * _moveSpeed;
                _playerRigidbody.velocity = new Vector3(limitedVelocity.x, _playerRigidbody.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;

        _playerRigidbody.velocity = new Vector3(_playerRigidbody.velocity.x, 0f, _playerRigidbody.velocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    public void JumpAction()
    {
        if (_isJumping && _readyToJump && _isGrounded)
        {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void ResetJump()
    {
        _readyToJump = true;

        _exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _raycastLenght))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }
}
