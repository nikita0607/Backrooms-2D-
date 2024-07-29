using BHSCamp;
using BHSCamp.FSM;
using UnityEngine;
public class Walk : FsmState
{
    private Move _move;
    private WiremanController _controller;
    private float _timer;

    public Walk(Fsm fsm, WiremanController controller, Move move) : base(fsm)
    {
        _controller = controller;
        _move = move;
    }

    public override void Enter() {
        _timer = _controller.IdleWalkTime;
    }

    public override void Update(float deltaTime) {
        float direction = _controller.Direction;
        _move.SetVelocity(new Vector2(direction, 0), _controller.WalkSpeed);
        _timer -= deltaTime;

        if (_controller.PlayerInSight())
            Fsm.SetState<RunToPlayer>();
        
        if (_timer <= 0)
            Fsm.SetState<WaitingSpawn>();
    }
}
