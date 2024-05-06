using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) 
    {
        _isRootState = true;
        InitializeSubState();
    }

public override void EnterState()
    {
        //enter animation for eight cardinal directions
        _ctx.CurrentMovementY = _ctx.GroundedGravity;
        _ctx.AppliedMovementY = _ctx.GroundedGravity;
        InitializeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        if (!_ctx.IsMovePressed)
        {
            _ctx.HandleAnimation(_ctx.IdleAnim); //#quick fix grounded to substate animation transition
            SetSubState(_factory.Idle());
        }
        else
        {
            _ctx.HandleAnimation(_ctx.WalkAnim); //#quick fix grounded to substate animation transition
            SetSubState(_factory.Walk());
        }
    }

    public override void CheckSwitchState()
    {
        //if the player is grounded and jump is pressed, switch to jumpState;
        if (_ctx.IsJumpPressed)
        {
            SwitchState(_factory.Jump());
        }
    }
}
