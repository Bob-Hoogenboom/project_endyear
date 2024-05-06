using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //enter animation for eight cardinal directions
        _ctx.HandleAnimation(_ctx.WalkAnim);
    }

    public override void UpdateState()
    {
        CheckSwitchState();

        _ctx.AppliedMovementX = _ctx.CurrentMovementX;
        _ctx.AppliedMovementZ = _ctx.CurrentMovementZ;
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if (!_ctx.IsMovePressed)
        {
            SwitchState(_factory.Idle());
        }
    }
}
