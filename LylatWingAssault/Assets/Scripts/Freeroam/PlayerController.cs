using UnityEngine;
using System.Collections;

public enum States
{
    Idle,
    Walk,
    Jump
}


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController _characterController;
    private Transform _cam;

    [Header("PlayerStats")]
    [SerializeField] private float speed = 5f;

    [SerializeField] private float _initialJumpVelocity;
    [SerializeField] private float maxJumpheight = 1.0f;
    [SerializeField] private float maxJumpTime = 0.5f;


    [Header("Constants")]
    private float _gravity = -9.8f;
    private float _groundedGravity = -0.05f;


    [Header("Input")]
    [SerializeField]private float _turnSmoothTime = 0.1f;

    private Vector3 currentMovement;
    private Vector3 _appliedMovement;
    private bool _moving;
    private bool _isJumpPressed;
    private bool _isJumping;


    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _cam = Camera.main.transform;
        SetJumpVariables();
    }

    private void Update()
    {
        _isJumpPressed = Input.GetKey(KeyCode.Space);
        currentMovement.x = Input.GetAxisRaw("Horizontal");
        currentMovement.z = Input.GetAxisRaw("Vertical");
        _moving = currentMovement.x != 0 || currentMovement.z != 0;


        HandleRotation();
        
        Move();

        ApplyGravity();
        HandleJump();
    }

    private void SetJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        _gravity = (-2 * maxJumpheight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * maxJumpheight) / timeToApex;
    }

    private void HandleJump()
    {
        if (!_isJumping && _characterController.isGrounded && Input.GetKey(KeyCode.Space))
        {
            _isJumping = true;
            currentMovement.y = _initialJumpVelocity;
            _appliedMovement.y = _initialJumpVelocity;
        }
        else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
        }
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_moving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _turnSmoothTime * Time.deltaTime);
        }
    }

    private void Move()
    {
        _appliedMovement.x = currentMovement.x;
        _appliedMovement.z = currentMovement.z;

        _characterController.Move(_appliedMovement * speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !_isJumpPressed;
        float fallMultiplier = 2.0f;

        if (_characterController.isGrounded)
        {
            currentMovement.y = _groundedGravity;
        } 
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (_gravity * fallMultiplier * Time.deltaTime);
            _appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * 0.5f, -20.0f);
        }
        else
        {
            //Velocity Verlet instead of Euler integration
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y +  (_gravity * Time.deltaTime);
            _appliedMovement.y = (previousYVelocity + currentMovement.y) * 0.5f;
        }
    }
}
