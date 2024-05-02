public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

public override void EnterState()
    {
        _ctx.CurrentMovementY = _ctx.GroundedGravity;
        _ctx.AppliedMovementY = _ctx.GroundedGravity;
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
