using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    WiremanController _controller;
    public event Action DirectionChanged;

    private void Awake() {
        _controller = GetComponent<WiremanController>();
    }

    private void OnCollisionStay2D(Collision2D other) {
        for (int contactNum=0; contactNum<other.contactCount; contactNum++) {
            ContactPoint2D contact = other.GetContact(contactNum);
            if (Mathf.Abs(contact.normal.x) > 0.9f) {
                _controller.SetDirection(-_controller.Direction);
                DirectionChanged?.Invoke();
                break;
            }
        }
    }
    
}
