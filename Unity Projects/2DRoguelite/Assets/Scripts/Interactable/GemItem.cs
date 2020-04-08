using UnityEngine;

public class GemItem : InteractableItem
{
    [SerializeField] private int gemWorth;
    [SerializeField] private AudioClip gemSound;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
            Interact(coll.GetComponent<PlayerController>());
    }

    override public void Interact(PlayerController playerController)
    {
        AudioManager.current.PlaySFX(gemSound);

        GameManager.current.PlayerGems += gemWorth;
        UIManager.current.updateGemsUICallback.Invoke();

        Destroy(gameObject);
    }
}
