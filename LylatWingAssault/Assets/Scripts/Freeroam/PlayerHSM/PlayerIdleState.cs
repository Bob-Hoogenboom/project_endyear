using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //enter animation for eight cardinal directions
        _ctx.HandleAnimation(_ctx.IdleAnim);

        _ctx.AppliedMovementX = 0;
        _ctx.AppliedMovementZ = 0;
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
        if (_ctx.IsMovePressed)
        {
            SwitchState(_factory.Walk());
        }
    }
}
