using System.Collections;
using System.Collections.Generic;
using BHSCamp.FSM;
using UnityEngine;

public class WaitingSpawn : FsmState
{
    private WiremanController _controller;
    private float _spawnDelay;
    private float _timer;

    public WaitingSpawn(Fsm fsm, WiremanController controller) : base(fsm)
    {
        _controller = controller;
        _spawnDelay = Random.Range(_controller.MinSpawnTimer, _controller.MaxSpawnTimer);
    }

    public override void Enter() {
        SetEnabled(false);
    }

    private void SetEnabled(bool enabled) {
        _controller.GetComponent<SpriteRenderer>().enabled = enabled;
        foreach (Collider2D coll in _controller.GetComponents<Collider2D>())
            coll.enabled = enabled;
    }

    private GameObject GetRandomSpawn() {
        return _controller.Spawns[Random.Range(0, _controller.Spawns.Length)];
    }

    public override void Update(float deltaTime)
    {
        _timer += deltaTime;

        if (_timer >= _spawnDelay) {
            _timer = 0;
            SetEnabled(true);
            _controller.gameObject.transform.position = GetRandomSpawn().transform.position;
            Fsm.SetState<Walk>();
        }
    }
}
