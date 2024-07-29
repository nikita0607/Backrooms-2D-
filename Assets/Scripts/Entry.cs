using System.Collections;
using UnityEngine;

public class Entry : Interactable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _nextEntry;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private AnimationClip _playerLightAnimation;


    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        Interaction interaction = other.gameObject.GetComponent<Interaction>();
        if (interaction == null) return;

        interaction.SetInterractionCallback(Transit);
    }

    protected override void OnTriggerExit2D(Collider2D other) {
        base.OnTriggerExit2D(other);

        Interaction interaction = other.gameObject.GetComponent<Interaction>();
        if (interaction == null) return;

        interaction.RemoveInterractionCallback(Transit);
    }

    public void Transit(GameObject target) {
        float delayTimer = _playerLightAnimation.length/2;

        if ((_playerMask.value & (1 << target.gameObject.layer)) != 0) {
            _playerAnimator.Play(_playerLightAnimation.name);
        }
        StartCoroutine(CoroutineTransition(delayTimer, target));
    }

    private IEnumerator CoroutineTransition(float delayTimer, GameObject target) {
            target.GetComponent<Interaction>().RemoveInterractionCallback(Transit);
            yield return new WaitForSeconds(delayTimer);

            Vector3 offset = transform.position - target.transform.position;
            offset = new Vector3(0, transform.position.y - target.transform.position.y, 0);
            target.transform.position = _nextEntry.transform.position - offset;
    }
}
