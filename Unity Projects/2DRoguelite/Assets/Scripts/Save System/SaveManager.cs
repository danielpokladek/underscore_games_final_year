using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager current = null;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);
    }
    #endregion

    private GameObject playerRef;
    private PlayerStats playerStats;

    public void Save()
    {
        playerRef = GameManager.current.playerRef;
        playerStats = playerRef.GetComponent<PlayerStats>();

        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream saveFile = File.Open(Application.persistentDataPath + "/" + "TestSave.dat", FileMode.Create);

            SaveData data = new SaveData();

            SavePlayer(data);

            binaryFormatter.Serialize(saveFile, data);
            saveFile.Close();
        }
        catch (System.Exception)
        {
            // Use this to handle errors.
        }
    }

    public void Load()
    {
        playerRef = GameManager.current.playerRef;
        playerStats = playerRef.GetComponent<PlayerStats>();

        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream saveFile = File.Open(Application.persistentDataPath + "/" + "TestSave.dat", FileMode.Open);

            SaveData data = (SaveData)binaryFormatter.Deserialize(saveFile);

            saveFile.Close();

            LoadPlayer(data);
        }
        catch (System.Exception)
        {

        }
    }

    private void SavePlayer(SaveData data)
    {
        //string itemList = "";

        //foreach (PlayerItem item in playerRef.GetComponent<PlayerController>().itemsList)
        //{
        //    itemList = itemList + item.itemID + ",";
        //}

        data.playerData = new PlayerData(
            playerStats.currentHealth,
            playerStats.characterHealth.GetValue(),
            playerStats.characterSpeed.GetValue(),
            playerStats.characterAttackDamage.GetValue(),
            playerStats.characterAttackDelay.GetValue());
    }

    private void LoadPlayer(SaveData data)
    {
        //string[] split = data.playerData.ItemsList.Split(","[0]);
        //PlayerItem tempItem;

        //foreach (string str in split)
        //{
        //    Debug.Log(str);

        //    foreach (ShopPlayerModifier item in GameManager.current.masterItemList.playerItems)
        //    {
        //        if (str == item.itemID.ToString())
        //        {
        //            tempItem = item.LoadItem();
        //            tempItem.name = item.itemName;

        //            playerRef.GetComponent<PlayerController>().itemsList.Add(tempItem);

        //            Debug.Log("Item found: " + item.itemName);
        //            break;
        //        }

        //    }
        //}

        playerStats.LoadStats(
            data.playerData.CurrentHealth,
            data.playerData.MaxHealth,
            data.playerData.MoveSpeed,
            data.playerData.AttackDamage,
            data.playerData.AttackDelay);
    }
}
