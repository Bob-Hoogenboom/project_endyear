using EightDirectionalSpriteSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerStateMachine : MonoBehaviour
{
    [Header("States")]
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    // >>>>>[Header("Getters & Setters")]
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState {  get { return _currentState; } set { _currentState = value; } }
    
    public float InitialJumpVelocity { get { return initialJumpVelocity; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
    public bool IsMovePressed { get { return _isMovePressed; } }

    //#Vector3 get/Set?
    public float CurrentMovementX { get { return _currentMovement.x; } set { _currentMovement.x = value; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float CurrentMovementZ { get { return _currentMovement.z; } set { _currentMovement.z = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } } 
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }

    public float Gravity { get { return _gravity; } }
    public float GroundedGravity { get { return _groundedGravity; } }

    //SpriteAnimation
    public ActorAnimation IdleAnim { get { return idleAnim; } }
    public ActorAnimation WalkAnim { get { return walkAnim; } }
    public ActorAnimation JumpAnim { get { return jumpAnim; } }
    public ActorAnimation FallAnim { get { return fallAnim; } }
    public ActorAnimation DeathAnim { get { return deathAnim; } }


    [Header("References")]
    [SerializeField] private Transform myTransform;
    private CharacterController _characterController;
    private Transform _cam;

    [Header("PlayerStats")]
    [SerializeField] private float speed = 7f;

    [SerializeField] private float initialJumpVelocity;
    [SerializeField] private float maxJumpheight = 1.0f;
    [SerializeField] private float maxJumpTime = 0.75f;

    [Header("SpriteAnimation")]
    [SerializeField] private  ActorBillboard actorBillboard;
    [Space]
    [SerializeField] private ActorAnimation idleAnim;
    [SerializeField] private ActorAnimation walkAnim;
    [SerializeField] private ActorAnimation jumpAnim;
    [SerializeField] private ActorAnimation fallAnim;
    [SerializeField] private ActorAnimation deathAnim;

    [Header("Constants")]
    [SerializeField] private float turnSmoothTime = 5f;
    private float _gravity = -9.8f;
    private float _groundedGravity = -2f;


    [Header("Input")]
    private Vector3 _currentMovement;
    private Vector3 _appliedMovement;
    private bool _isMovePressed;
    private bool _isJumpPressed;

    private bool _isJumping;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cam = Camera.main.transform;

        SetJumpVariables();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();

        HandleAnimation(idleAnim);

        _currentState.EnterState();
    }

    private void SetJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        _gravity = (-2 * maxJumpheight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpheight) / timeToApex;
    }

    private void FixedUpdate()
    {
        _isJumpPressed = Input.GetKey(KeyCode.Space);
        _currentMovement.x = Input.GetAxisRaw("Horizontal");
        _currentMovement.z = Input.GetAxisRaw("Vertical");
        _isMovePressed = _currentMovement.x != 0 || _currentMovement.z != 0;

        actorBillboard.SetActorForwardVector(myTransform.forward);

        Move();

        HandleRotation();

        _currentState.UpdateStates();
    }

    public void HandleAnimation(ActorAnimation newAnim)
    {
        //Debug.Log(newAnim);
        if (actorBillboard != null)
        {
            actorBillboard.PlayAnimation(newAnim);
        }
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _appliedMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = _appliedMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMovePressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, turnSmoothTime * Time.deltaTime);
        }
    }

    private void Move()
    {
        Vector3 camForward = _cam.forward;
        Vector3 camRight = _cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 forwardRelative = _currentMovement.z * camForward;
        Vector3 rightRelative = _currentMovement.x * camRight;

        Vector3 moveDir = forwardRelative + rightRelative;

        _appliedMovement.x = moveDir.x;
        _appliedMovement.z = moveDir.z;

        _characterController.Move(_appliedMovement * speed * Time.deltaTime);
    }
}
