﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataController : Singleton<DataController>
{
    #region Constructor & Variables

    protected DataController() { }

    private const string DEFAULT_SAVE_NAME = "SketchTracy_";

    [HideInInspector]
    public string fileName;


    #endregion

    #region New Game

    public void NewGame()
    {
        int saveSlotIndex = GetNextAvailableSaveSlot();
        fileName = DEFAULT_SAVE_NAME + saveSlotIndex;
        SaveGame();
        Debug.Log("File " + fileName + " created successfully!");
        Application.LoadLevel("Level 1");
    }

    private int GetNextAvailableSaveSlot()
    {
        bool saveSlotFound = false;
        int saveSlot = 0;

        while (!saveSlotFound)
        {
            if (!File.Exists(Application.persistentDataPath + "/" + DEFAULT_SAVE_NAME + saveSlot + ".dat"))
            {
                saveSlotFound = true;
            }
            else
            {
                saveSlot++;
            }
        }

        return saveSlot;
    }

    #endregion

    #region Save Game

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileName + ".dat");

        GameData data = new GameData();
        data.fileName = fileName;

        bf.Serialize(file, data);
        file.Close();
    }

    #endregion

    #region Load Game

    public void LoadGame(int pSaveSlotIndex)
    {
        string fileToLoad = DEFAULT_SAVE_NAME + pSaveSlotIndex;

        if (File.Exists(Application.persistentDataPath + "/" + fileToLoad + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileToLoad + ".dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            fileName = data.fileName;
            Debug.Log("File " + fileName + " loaded successfully!");
        }
        else
        {
            Debug.Log("File " + fileName + " does not exist!");
        }
    }

    #endregion
}


[Serializable]
class GameData
{
    public string fileName;
}
