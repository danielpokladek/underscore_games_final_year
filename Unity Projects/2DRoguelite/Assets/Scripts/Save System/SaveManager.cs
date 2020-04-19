using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        data.playerData = new PlayerData(
            playerStats.currentHealth,
            playerStats.characterHealth.GetValue(),
            playerStats.characterSpeed.GetValue(),
            playerStats.characterAttackDamage.GetValue(),
            playerStats.characterAttackDelay.GetValue());
    }

    private void LoadPlayer(SaveData data)
    {
        playerStats.SetHealth(data.playerData.CurrentHealth);
        playerStats.characterHealth.LoadValue(data.playerData.MaxHealth);
        playerStats.characterSpeed.LoadValue(data.playerData.MoveSpeed);
        playerStats.characterAttackDamage.LoadValue(data.playerData.AttackDamage);
        playerStats.characterAttackDelay.LoadValue(data.playerData.AttackDelay);
    }
}
