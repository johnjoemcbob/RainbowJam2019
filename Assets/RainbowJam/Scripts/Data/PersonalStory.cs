using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalStory
{
    //Just some goals..
    public enum PersonalGoals
    {
        PART_0 = -1,    //not met
        PART_1 = 0,     //talking in city
        PART_2 = 50,    //mid-point story in commune
        PART_3 = 100,   //leaving commune
        PART_4 = 101    //return for party
    }
    PersonalGoals currentStage = PersonalGoals.PART_0;

    int personalEXP = 0;
    float expGainRate = 1.0f; //Do people gain exp at diff rates?
    public int storyID = -1; //Get from json


    void Start()
    {
        //[TODO] ---> Get json story

        expGainRate = Random.Range(0.75f, 1.00f);
    }



    //Some quick getters
    public int GetCurrentEXP()
    {
        return personalEXP;
    }

    //Some quick getters
    public float GetEXPGainRate()
    {
        return expGainRate;
    }

    public PersonalGoals GetCurrentStage()
    {
        return currentStage;
    }

}