using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCollectionHandler : MonoBehaviour
{
    public int newFriendCount = 0;
    public int communeFriendCount = 0;
    private float communeMaxCount = 0;

    CrowdManager crowdManager;
    Text newFriendText;
    Text communeFriendText;


    // Start is called before the first frame update
    void Start()
    {
        //Get crowdmanager
        GameObject crowdM = GameObject.Find("CrowdManager");

        if (crowdM != null)
        {
            crowdManager = crowdM.GetComponent<CrowdManager>();
        }
        
        
        
        //Get text
        GameObject newFriendObj = GameObject.Find("NewFriendCount");

        if (newFriendObj != null)
        {
            newFriendText = newFriendObj.GetComponent<Text>();
        }

        GameObject communeObj = GameObject.Find("AvailableSpaceCount");

        if (communeObj != null)
        {
            communeFriendText = communeObj.GetComponent<Text>();
        }

        UpdateFriendNumbers();
        UpdateCommuneNumbers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetNumbers()
    {
        newFriendCount = 0;
        communeFriendCount = 0;
        communeMaxCount = 0;

        UpdateFriendNumbers();
        UpdateCommuneNumbers();
    }

    public void CollectNewFriend()
    {
        //[TODO] --> some logic for the commune maybe?

        newFriendCount += 1;

        UpdateFriendNumbers();
        UpdateCommuneNumbers();
    }


    public void UpdateFriendNumbers()
    {
        if ((newFriendText != null) && (crowdManager != null))
        {
            newFriendText.text = newFriendCount + "/" + crowdManager.friendPoolSize;
        }
    }

    public void UpdateCommuneNumbers()
    {
        if (communeFriendText != null)
        {
            communeFriendText.text = (newFriendCount + communeFriendCount) + "/" + (int)communeMaxCount;
        }
    }


    public void SetCommuneVariables(int current, float max)
    {
        //Debug.Log("CURRENT: " + current + "   " + "MAX: " + max);

        communeFriendCount = current;
        communeMaxCount = max;

        UpdateCommuneNumbers();
    }

    public bool IsCommuneFull()
    {
        bool full = false;

        int totalFriends = communeFriendCount + newFriendCount;

        if (totalFriends >= (int)communeMaxCount)
            full = true;

        return full;
    }
}
