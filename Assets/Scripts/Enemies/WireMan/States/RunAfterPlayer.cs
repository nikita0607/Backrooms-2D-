using BHSCamp;
using BHSCamp.FSM;
using UnityEngine;

public class RunAfterPlayer : FsmState
{
    private WiremanController _controller;
    private Jump _jump;
    private Move _move;
    private Vector2 _targetPostition;
    private DirectionChanger _directionChanger;
    private Interaction _interaction;
    private PlayerInteractionChecker _interactionChecker;
    private float _timer;
    private Vector2 _direction;

    private float _speed;


    public RunAfterPlayer(Fsm fsm, Move move, Jump jump, WiremanController controller, DirectionChanger directionChanger, Interaction interaction) : base(fsm)
    {
        _controller = controller;
        _move = move;
        _jump = jump;

        _directionChanger = directionChanger;
        _interaction = interaction;
        _interactionChecker = _controller.GetComponent<PlayerInteractionChecker>();
    }

    public void OnDirectionChange() {
        _direction = DirectionToTarget();
        _controller.SetDirection((int)_direction.x);
        _jump.Action();
    }

    public override void Enter() {
        _controller.Source.Stop();
        _timer = _controller.SeekTargetTime;
        _targetPostition = _controller.LastPlayerPosition;
        _directionChanger.DirectionChanged += OnDirectionChange;

        _direction = DirectionToTarget();
    }

    public override void Exit() {
        _directionChanger.DirectionChanged -= OnDirectionChange;
    }

    private Vector2 DirectionToTarget() {
        Vector2 direction = _targetPostition - (Vector2)_controller.transform.position;
        direction.y = 0;
        direction = direction.normalized;

        return direction;
    }

    // Update is called once per frame
    public override void Update(float deltaTime)
    {
        Debug.Log("GEFR");

        if (_controller.PlayerInSight()) 
        {
            Fsm.SetState<RunToPlayer>();
            return;
        }

        _timer -= deltaTime;

        if (_timer <= 0) {
            Fsm.SetState<WaitingSpawn>();
            return;
        }
        

        _speed = _controller.FastWalkSpeed;

        _controller.SetDirection((int)_direction.x);
        _move.SetVelocity(_direction, _speed);

        if (DirectionToTarget() != _direction) {
            if (_interaction._interractCallback == _interactionChecker.LastAction) {
                _interaction.Interract();
                _interaction.RemoveInterractionCallback(_interaction._interractCallback);
                _direction = new(Random.Range(0, 2) == 0 ? -1 : 1, 0);
                return;
            }
            Fsm.SetState<Walk>();
        }
    }
}
