using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject _interactPrefub;
    [SerializeField] protected LayerMask _playerMask;
    [SerializeField] private Vector2 _uiInteractOffset;

    protected GameObject _interactImage;

    void Start() {
        _interactImage = Instantiate(_interactPrefub, DrawPosition(), Quaternion.identity);
        _interactImage.SetActive(false);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if ((_playerMask.value & (1 << other.gameObject.layer)) == 0) return;
        _interactImage.SetActive(true);
    }   

    protected virtual void OnTriggerExit2D(Collider2D other) {
        if (_interactImage == null) return;
        _interactImage.SetActive(false);
    }

    private Vector2 DrawPosition() {
        return (Vector2)gameObject.transform.position + _uiInteractOffset;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(DrawPosition(), 0.5f);
    }
}
