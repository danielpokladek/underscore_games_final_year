using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public PlayerItem playerItem;
    public int itemID;
    public string itemName;
    [TextArea(2, 5)]
    public string itemDesc;
    [SerializeField] protected int itemPrice;
    [SerializeField] protected bool isDungeonItem = false;

    protected bool isDungeonChest;
    protected bool activated = false;

    private void Start()
    {
        if (itemID == 0)
            Debug.LogError("ItemID not set on: " + gameObject.name + ". Item won't be saved, causing errors!");

        playerItem.SetItem(itemName, itemDesc, itemID);
    }

    public void Item(string _itemName, int _itemPrice, bool _isDungeonItem = false)
    {
        itemPrice = _itemPrice;
        itemName  = _itemName;
        isDungeonItem = _isDungeonItem;
    }

    virtual public PlayerItem LoadItem()
    {
        return null;
    }

    virtual public void Interact(PlayerController playerController)
    {
        Debug.Log("It just works! - Todd Howard");
    }

    protected bool CheckGems()
    {
        if (itemPrice == 0)
            return true;

        if ((GameManager.current.PlayerGems - itemPrice) >= 0)
        {
            GameManager.current.PlayerGems -= itemPrice;
            UIManager.current.updateUICallback.Invoke();
            UIManager.current.updateGemsUICallback.Invoke();

            return true;
        }

        return false;
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if ((GameManager.current.PlayerGems - itemPrice) >= 0)
                GameUIManager.currentInstance.ShowItemUI(transform.position, itemName, itemPrice, Color.white, isDungeonItem);
            else
                GameUIManager.currentInstance.ShowItemUI(transform.position, itemName, itemPrice, Color.red, isDungeonItem);

            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 5.0f);
        }
    }

    virtual protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIManager.currentInstance.HideItemUI();
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.0f);
        }
    }
}
