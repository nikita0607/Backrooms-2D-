using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiremanAnimation : MonoBehaviour
{
    private Rigidbody2D _body;
    private Animator _animator;

    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("VelocityX", Mathf.Abs(_body.velocity.x));
    }
}
