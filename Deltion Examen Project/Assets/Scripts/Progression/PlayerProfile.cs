using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class PlayerProfile : MonoBehaviour
{
    public PlayerTemplate template;
	private string filePath;
    public int level;
    public Dictionary<int, float> levelExpDictionary = new Dictionary<int, float>();
    public static PlayerProfile instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
		filePath = Application.persistentDataPath;
        Initialize();
    }

    public void SetUserName(string newUsername)
    {
        template.username = newUsername;
        SaveToDisk(template, filePath);
    }

    private void SetLevelsRequiredExp()
    {
        float requiredExp = 0;
        for (int i = 0; i < 51; i++)
        {
            levelExpDictionary.Add(i, requiredExp);
            requiredExp += i * 1000;
        }
    }

    public void RecieveExp(float amount)
    {
        template.xp += amount;
        SaveToDisk(template, filePath);
        SetLevel();
    }

    private void SetLevel()
    {
        float currentExp = template.xp;

        foreach (var kvp in levelExpDictionary)
        {
            if (currentExp >= kvp.Value)
            {
                level = kvp.Key;
            }
        }
    }

    private void Initialize()
    {
        SetLevelsRequiredExp();
        filePath = filePath + @"\" + "profile.txt";
        if (!File.Exists(filePath))
        {
            template.xp = 0;
            level = 1;
            SetLevel();
            template.doneTutorial = false;
            template.username = "Username";
            SaveToDisk(template, filePath);
        }
        else
        {
            LoadFromDisk(filePath);
            SetLevel();
            Debug.Log(level);
        }

    }

    public void SaveToDisk(PlayerTemplate data, string path)
    {
        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
    }

    public  void LoadFromDisk(string path)
    {
        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            template = JsonUtility.FromJson<PlayerTemplate>(jsonString);
        }
    }
}

[System.Serializable]
public class PlayerTemplate
{
    //public int level;
    public float xp;
    public string username;
    public bool doneTutorial;
}
