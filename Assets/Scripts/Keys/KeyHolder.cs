using System.Collections;
using System.Collections.Generic;
using BHSCamp;
using UnityEngine;

public class KeyHolder : Interactable
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private AnimationClip _hasKeyAnimation;
    [SerializeField] private AnimationClip _hasNoKeyAnimation;
    public bool HasKey;


    private void Awake() {
        _animator = GetComponent<Animator>();
    }


    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        Interaction interaction = other.gameObject.GetComponent<Interaction>();
        if (interaction == null) return;

        interaction.SetInterractionCallback(GetKey);
    }

    protected override void OnTriggerExit2D(Collider2D other) {
        base.OnTriggerExit2D(other);

        Interaction interaction = other.gameObject.GetComponent<Interaction>();
        if (interaction == null) return;

        interaction.RemoveInterractionCallback(GetKey);
    }

    private void GetKey(GameObject target) {
        if (HasKey) {
            GameManager.Instance.AddScore(1); 
            _animator.Play(_hasKeyAnimation.name);
            HasKey = false;
        }
        else {
            _animator.Play(_hasNoKeyAnimation.name);
        }
    }
}
