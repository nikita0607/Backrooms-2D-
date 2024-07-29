using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BHSCamp;
using BHSCamp.FSM;
using UnityEditor;

public class WiremanController : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] public float MinSpawnTimer;
    [SerializeField] public float MaxSpawnTimer;
    [SerializeField] public GameObject[] Spawns;

    [Header("RunToPlayer Timer Settings")]
    [SerializeField] public float IdleWalkTime;
    [SerializeField] public float RunTargetTime;
    [SerializeField] public float SeekTargetTime;
    [SerializeField] public AudioClip RunToPlayerStartClip;
    public AudioSource Source;

    [Header("Other")]
    [SerializeField] public float WalkSpeed;
    [SerializeField] public float RunSpeed;
    [SerializeField] public float FastWalkSpeed;
    [SerializeField] private Vector2 _watchRange;
    [SerializeField] private Vector2 _watchOffset;
    [SerializeField] private LayerMask _playerLayerMask;

    public Vector2 CenterOffset;
    private Move _move;
    private Jump _jump;

    public Fsm fsm;

    public int Direction {get; private set;}
    public Vector2 LastPlayerPosition {get; private set;}


    void Awake()
    {
        _move = GetComponent<Move>();
        _jump = GetComponent<Jump>();
        Source = GetComponent<AudioSource>();
    }
    void Start()
    {
        Direction = transform.localScale.x > 0 ? 1 : -1;
        fsm = new();
        fsm.AddState(new Walk(fsm, this, _move));
        fsm.AddState(new RunToPlayer(fsm, _move, _jump, this, GetComponent<DirectionChanger>(), GetComponent<Interaction>()));
        fsm.AddState(new RunAfterPlayer(fsm, _move, _jump, this, GetComponent<DirectionChanger>(), GetComponent<Interaction>()));
        fsm.AddState(new WaitingSpawn(fsm, this));

        fsm.SetState<WaitingSpawn>();

        GameManager.OnDifficultyChanged += OnDifficultyChanged;
        OnDifficultyChanged(GameManager.Instance.Difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update(Time.deltaTime);
    }

    public void SetDirection(int direction) {
        Direction = direction;
        float xScale = Mathf.Abs(transform.localScale.x)*direction;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }

    public void OnDifficultyChanged(int difficulty) {
        if (difficulty == 0) gameObject.SetActive(false);
        else {
            if (!gameObject.activeSelf) {
                fsm.SetState<WaitingSpawn>();
            }
            gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float direction = Direction != 0 ? Direction : 1;

        Vector2 origin = new(
            transform.position.x + (direction * (_watchRange.x + _watchOffset.x)/ 2),
            transform.position.y + _watchOffset.y
        );
        Gizmos.DrawWireCube(origin, _watchRange);
        Gizmos.DrawSphere((Vector2)transform.position+CenterOffset, 0.3f);
    }

    public virtual bool PlayerInSight()
    {
        RaycastHit2D hit = CheckPlayerHit();
        if (!hit) return false;
        LastPlayerPosition = hit.collider.gameObject.transform.position;
        return hit.collider.GetComponent<IDamageable>() != null;
    }

    public virtual RaycastHit2D CheckPlayerHit()
    {
        Vector2 origin = new(
            transform.position.x + (Direction * (_watchRange.x + _watchOffset.x) / 2),
            transform.position.y + _watchOffset.y
        );
        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            _watchRange,
            0f,
            new Vector2(Direction, 0),
            0,
            _playerLayerMask
        );

        return hit;
    }

    private void OnDestroy() {
        GameManager.OnDifficultyChanged -= OnDifficultyChanged;
    }
}
