using BHSCamp;
using BHSCamp.FSM;
using UnityEngine;

public class RunToPlayer : FsmState
{
    private WiremanController _controller;
    private Jump _jump;
    private Move _move;
    private GameObject _target;
    private DirectionChanger _directionChanger;
    private Interaction _interaction;
    private float _timer;
    private Vector2 _direction;

    private float _speed;


    public RunToPlayer(Fsm fsm, Move move, Jump jump, WiremanController controller, DirectionChanger directionChanger, Interaction interaction) : base(fsm)
    {
        _controller = controller;
        _move = move;
        _jump = jump;

        _directionChanger = directionChanger;
        _interaction = interaction;
    }

    public void OnDirectionChange() {
        _direction = DirectionToTarget(_target);
        _controller.SetDirection((int)_direction.x);
        _jump.Action();
    }

    public override void Enter() {
        _target = null;
        _timer = _controller.RunTargetTime;
        _directionChanger.DirectionChanged += OnDirectionChange;

        _controller.Source.PlayOneShot(_controller.RunToPlayerStartClip);
    }

    public override void Exit() {
        _directionChanger.DirectionChanged -= OnDirectionChange;
    }

    private Vector2 DirectionToTarget(GameObject target) {
        Vector2 direction = _target.transform.position - (Vector3)_controller.CenterOffset - _controller.transform.position;
        direction.y = 0;
        direction = direction.normalized;

        return direction;
    }

    // Update is called once per frame
    public override void Update(float deltaTime)
    {
        CheckTarget();

        if (!_controller.PlayerInSight() || _target==null) {
            Fsm.SetState<RunAfterPlayer>();
            return;
        }

        _timer -= deltaTime;
        if (_timer <= 0) {
            Fsm.SetState<WaitingSpawn>();
            return;
        }

        _direction = DirectionToTarget(_target);
        _speed = _controller.RunSpeed;

        _controller.SetDirection((int)_direction.x);
        _move.SetVelocity(_direction, _speed);
    }

    private void CheckTarget() {
        if (_target != null) return;
        _target = _controller.CheckPlayerHit().collider?.gameObject;

        if (_target == null) {
            _controller.SetDirection(_controller.Direction);
            _target = _controller.CheckPlayerHit().collider?.gameObject;
        }

    }
}
