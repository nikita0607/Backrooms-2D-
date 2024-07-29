using TMPro;
using UnityEngine;

public class NextLevel : Interactable
{
    // Start is called before the first frame update
    [SerializeField] private Menu _menu;

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
        _menu.SendMessage("next");
    }
}
