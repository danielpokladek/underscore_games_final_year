using UnityEngine;

public class GemItem : InteractableItem
{
    [SerializeField] private int gemWorth;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
            Interact(coll.GetComponent<PlayerController>());
    }

    override public void Interact(PlayerController playerController)
    {
        GameManager.current.PlayerCurrency += gemWorth;
        Destroy(gameObject);
    }
}
