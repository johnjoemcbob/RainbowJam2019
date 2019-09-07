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
    PersonalGoals currentGoal = PersonalGoals.PART_0;

    float personalEXP = -1.0f;
    float expGainRate = 1.0f; //Do people gain exp at diff rates?
    public int storyID = -1; //Get from json


    public static PersonalStory GenerateRandom()
    {
        var newStory = new PersonalStory();

        //[TODO] ---> Get json story

        newStory.personalEXP = -1; //Just in case
        newStory.currentGoal = PersonalGoals.PART_0;
        newStory.expGainRate = Random.Range(0.75f, 1.10f);

        return newStory;
    }




    // THIS MAKES MY LIFE EASIER -----------------------------------------
    PersonalGoals GetNextGoal(PersonalGoals current)
    {
        switch (current)
        {
            case (PersonalGoals.PART_0):
                {
                    return PersonalGoals.PART_1;
                }
            case (PersonalGoals.PART_1):
                {
                    return PersonalGoals.PART_2;
                }
            case (PersonalGoals.PART_2):
                {
                    return PersonalGoals.PART_3;
                }
            case (PersonalGoals.PART_3):
                {
                    return PersonalGoals.PART_4;
                }
            default:
                {
                    break;
                }
        }

        return current;
    }



    // SETTERS -----------------------------------------
    public void AddEXP(float amount)
    {
        //Add exp
        float actualAmount = amount * expGainRate * Time.deltaTime;

        personalEXP += actualAmount;

        //Check against goal thresholds
        if (currentGoal != PersonalGoals.PART_4)
        {
            PersonalGoals nextGoal = GetNextGoal(currentGoal);

            if (personalEXP >= (float)nextGoal)
            {
                SetNewGoal(nextGoal);
            }
        }
    }



    void SetNewGoal(PersonalGoals goal)
    {
        currentGoal = goal;
        OnGoalChange();
        Debug.Log("MY STORY HAS UPDATED:  " + currentGoal.ToString());
    }



    void OnGoalChange()
    {
        switch (currentGoal)
        {
            case (PersonalGoals.PART_1):
                {
                    //Just accepted commune invite
                    break;
                }
            case (PersonalGoals.PART_2):
                {
                    //Mid-point, approaches player in commune to talk
                    break;
                }
            case (PersonalGoals.PART_3):
                {
                    //End-point, approaches player in commune to talk, then leaves
                    break;
                }
            case (PersonalGoals.PART_4):
                {
                    //Party time
                    break;
                }
            default:
                {
                    break;
                }
        }
    }



    // GETTERS -----------------------------------------
    public float GetCurrentEXP()
    {
        return personalEXP;
    }

    public float GetEXPGainRate()
    {
        return expGainRate;
    }

    public PersonalGoals GetCurrentStage()
    {
        return currentGoal;
    }

}