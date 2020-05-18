using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class PlayerProfile : MonoBehaviour
{
    public PlayerTemplate template;
	private string filePath;

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
		filePath = Path.Combine(Application.persistentDataPath, "profile.txt");
        Initialize();
    }

    private void Initialize()
    {
        string path = filePath + @"\" + "profile.txt";
        if (!File.Exists(path))
        {
            template.xp = 0;
            template.doneTutorial = false;
            template.username = "Username";
            SaveToDisk(template, filePath);
        }
        else
        {
            LoadFromDisk(path);
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
    public int xp;
    public string username;
    public bool doneTutorial;
}
