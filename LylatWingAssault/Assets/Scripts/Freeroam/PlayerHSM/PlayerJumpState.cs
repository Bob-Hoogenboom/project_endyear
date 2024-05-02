using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        HandleGravity();
    }

    public override void ExitState()
    {
        _ctx.IsJumping = false;
    }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchState()
    {
        if (_ctx.CharacterController.isGrounded)
        {
            SwitchState(_factory.Grounded());
        }
    }

    private void HandleJump()
    {
        _ctx.IsJumping = true;
        _ctx.CurrentMovementY = _ctx.InitialJumpVelocity;
        _ctx.AppliedMovementY = _ctx.InitialJumpVelocity;
    }

    private void HandleGravity()
    {
        bool isFalling = _ctx.CurrentMovementY <= 0.0f || !_ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling)
        {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY = _ctx.CurrentMovementY + (_ctx.Gravity * fallMultiplier * Time.deltaTime);
            _ctx.AppliedMovementY = Mathf.Max((previousYVelocity + _ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        else
        {
            //Velocity Verlet instead of Euler integration
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY = _ctx.CurrentMovementY + (_ctx.Gravity * Time.deltaTime);
            _ctx.AppliedMovementY = (previousYVelocity + _ctx.CurrentMovementY) * 0.5f;
        }
    }
}
