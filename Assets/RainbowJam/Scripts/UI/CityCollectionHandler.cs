using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCollectionHandler : MonoBehaviour
{
    public int newFriendCount = 0;
    public int communeFriendCount = 0;

    CrowdManager crowdManager;
    Text newFriendText;


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CollectNewFriend()
    {
        //[TODO] --> some logic for the commune maybe?

        newFriendCount += 1;

        if ((newFriendText != null) && (crowdManager != null))
        {
            newFriendText.text = newFriendCount + "/" + crowdManager.friendPoolSize;
        }
    }

}
