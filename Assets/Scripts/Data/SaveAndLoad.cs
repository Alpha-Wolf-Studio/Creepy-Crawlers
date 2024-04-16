using System.Collections.Generic;
using UnityEngine;

public static class SaveAndLoad
{
    public static SaveGame SaveGame { get; private set; }
    private const string valuePath = "Level";
    private const string levelsPath = "MaxLevels";

    public static void SaveLevel(int lvl, int stars)
    {
        string key = valuePath + lvl;
        if (PlayerPrefs.HasKey(key))
        {
            int savedStars = PlayerPrefs.GetInt(key);
            if (stars > savedStars)
            {
                PlayerPrefs.SetInt(key, stars);
            }
        }
        else
            PlayerPrefs.SetInt(key, stars);

        if (PlayerPrefs.HasKey(levelsPath))
        {
            int savedLevels = PlayerPrefs.GetInt(levelsPath);
            if (savedLevels > lvl)
            {
                PlayerPrefs.SetInt(levelsPath, lvl);
            }
        }
        else
            PlayerPrefs.SetInt(levelsPath, lvl);
    }

    public static SaveGame LoadAll()
    {
        bool gettingData = true;
        int lvl = 1;
        SaveGame = new SaveGame();

        do
        {
            string key = valuePath + lvl;
            if (PlayerPrefs.HasKey(key))
            {
                int stars = PlayerPrefs.GetInt(key);
                SaveGame.level.Add(new Level(lvl, stars));
            }
            else
            {
                gettingData = false;
            }

            lvl++;
        } while (gettingData);

        if (PlayerPrefs.HasKey(levelsPath))
        {
            int maxLevels = PlayerPrefs.GetInt(levelsPath);
            SaveGame.maxLevel = maxLevels;
        }

        return SaveGame;
    }
}

public class SaveGame
{
    public int maxLevel = 1;
    public List<Level> level;

    public SaveGame()
    {
        level = new List<Level>();
    }
}

public class Level
{
    public int level;
    public int stars;

    public Level(int lvl, int stars)
    {
        this.level = lvl;
        this.stars = stars;
    }
}