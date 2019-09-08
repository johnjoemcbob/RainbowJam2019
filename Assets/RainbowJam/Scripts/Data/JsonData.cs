using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class JsonData : MonoBehaviour
{
    [Serializable]
    public struct StoryList
    {
        public string PART_1;
        public string PART_2;
        public string PART_3;
        public string PART_4;
    }

    [Serializable]
    public class StoryListContainer
    {
        public List<StoryList> storyList;

        public StoryListContainer()
        {
            storyList = new List<StoryList>();
        }
    }

    public static StoryListContainer storyDatabase;
    public static string[] nameDatabase;

    void Awake()
    {
        Debug.Log("LOADING DATA!");

        //GET NAMES
        string namesTxt = "";
        namesTxt = File.ReadAllText("Assets/RainbowJam/JSON/names.txt");
        nameDatabase = namesTxt.Split( new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);


        //GET STORIES
        string storyJson = "";
        storyJson = File.ReadAllText("Assets/RainbowJam/JSON/personalStories.json");

        storyDatabase = new StoryListContainer();
        storyDatabase = JsonUtility.FromJson<StoryListContainer>(storyJson);

        //Debug.Log(storyDatabase.storyList.Count);
    }



    // GET RANDOM NAME ---------------------------------------------------------------------
    public static string GetRandomName()
    {
        int id = 0;

        if (nameDatabase == null)
            return "DEBUG_FRIEND";

        if (nameDatabase.Length <= 0)
            return "DEBUG_FRIEND";

        //Randomise. Max is exclusive
        id = UnityEngine.Random.Range(0, nameDatabase.Length);

        return nameDatabase[id];
    }


    // GET RANDOM STORY ---------------------------------------------------------------------
    public static int GetRandomStoryID()
    {
        int storyID = 0;

        if (storyDatabase == null)
            return storyID;

        if (storyDatabase.storyList == null)
            return storyID;

        if (storyDatabase.storyList.Count <= 0)
            return storyID;

        //Randomise. Max is exclusive
        storyID = UnityEngine.Random.Range(0, storyDatabase.storyList.Count);

        return storyID;
    }


    // GET DIALOGUE ---------------------------------------------------------------------
    public static string GetDialogueFromStoryID(int id, PersonalStory.PersonalGoals part)
    {
        if (id < 0)
            return "story error: id below 0";

        if (storyDatabase == null)
            return "story error: no database container";

        if (storyDatabase.storyList == null)
            return "story error: no database";

        if (id >= storyDatabase.storyList.Count)
            return "story error: id larger than database count";


        //Actually find the part
        switch (part)
        {
            case (PersonalStory.PersonalGoals.PART_1):
                {
                    return storyDatabase.storyList[id].PART_1;
                }
            case (PersonalStory.PersonalGoals.PART_2):
                {
                    return storyDatabase.storyList[id].PART_2;
                }
            case (PersonalStory.PersonalGoals.PART_3):
                {
                    return storyDatabase.storyList[id].PART_3;
                }
            case (PersonalStory.PersonalGoals.PART_4):
                {
                    return storyDatabase.storyList[id].PART_4;
                }
            default:
                {
                    return "story error: no matching case";
                }
        }
    }
}
