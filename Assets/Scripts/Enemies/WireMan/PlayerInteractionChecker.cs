using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionChecker : MonoBehaviour
{
    [SerializeField] Interaction _playerInteraction;

    public Action<GameObject> LastAction;

    private WiremanController _controller;

    private void Awake() {
        _controller = GetComponent<WiremanController>();
    }
    void Start()
    {
        _playerInteraction.Interact += OnInteraction;
    }

    void OnInteraction(Action<GameObject> action, GameObject target) {
        if (_controller.PlayerInSight())
            LastAction = action;
    }
}
