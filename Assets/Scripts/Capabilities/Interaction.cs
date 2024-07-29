using System;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public Action<GameObject> _interractCallback {get; private set; }

    public event Action<Action<GameObject>, GameObject> Interact;

    public void SetInterractionCallback(Action<GameObject> interractCallback) {
        _interractCallback = interractCallback;
        Debug.Log($"{gameObject.name}: Inteaction Seted!");
    }

    public void RemoveInterractionCallback(Action<GameObject> interractCallback) {
        if (_interractCallback == interractCallback)
            _interractCallback = null;
    }

    public void Interract() {
        if (_interractCallback == null) return;

        Interact?.Invoke(_interractCallback, gameObject);
        _interractCallback(gameObject);
        Debug.Log($"{gameObject.name}: Interact!");
    }
}
