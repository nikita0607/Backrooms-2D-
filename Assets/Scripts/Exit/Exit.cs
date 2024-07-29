using System.Collections;
using System.Collections.Generic;
using BHSCamp;
using TMPro;
using UnityEngine;

public class Exit : Interactable
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private AnimationClip _notEnouthAnimationClip;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private AnimationClip _exitAnimationClip;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _keyCountForExit;
    [SerializeField] private Menu _menu;


    private void Awake() {
        _animator = GetComponent<Animator>();
        GameManager.Instance.SetScore(0);
    }


    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        Interaction interaction = other.gameObject.GetComponent<Interaction>();
        if (interaction == null) return;

        interaction.SetInterractionCallback(DoExit);
    }

    protected override void OnTriggerExit2D(Collider2D other) {
        base.OnTriggerExit2D(other);

        Interaction interaction = other.gameObject.GetComponent<Interaction>();
        if (interaction == null) return;

        interaction.RemoveInterractionCallback(DoExit);
    }

    private void DoExit(GameObject target) {
        if (GameManager.Instance.Score < _keyCountForExit) {
            _text.text = "You need " + (_keyCountForExit - GameManager.Instance.Score) + " more keys";
            _animator.Play(_notEnouthAnimationClip.name);
            return;
        }
        Debug.Log(_exitAnimationClip.name);
        _playerAnimator.Play(_exitAnimationClip.name);
        _interactImage.SetActive(false);
        Invoke(nameof(_nextLevel), _exitAnimationClip.length);
    }

    private void _nextLevel(){
        _menu.SendMessage("next");
    }
}
