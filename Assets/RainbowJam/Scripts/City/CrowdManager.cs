using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    PersonInfo personInfo;

    [SerializeField]
    private GameObject friendNPCPrefab;

    [SerializeField]
    private GameObject blankNPCPrefab;


    public int friendPoolSize = 5;
    public int totalCrowdSize = 100;


    // Start is called before the first frame update
    void Start()
    {
        //Spawn friends first
        for (int i = 0; i < friendPoolSize; i++)
        {
            //Spawn friends
            if (friendNPCPrefab != null)
            {
                GameObject friendObject = GameObject.Instantiate(friendNPCPrefab);
                var npcScript = friendObject.GetComponent<NPC>();
                npcScript.GenerateAppearanceFromData(PersonInfo.GenerateRandom("DEBUG_FRIEND"));

                friendObject.transform.SetParent(transform);

                friendObject.transform.Translate(0, 0, 0);
            }
        }


        //[TODO] Spawn false positives here


        //Spawn the remaining crowd as blank
        int remainingCrowd = totalCrowdSize - friendPoolSize;

        for (int i = 0; i < remainingCrowd; i++)
        {
            Debug.Log("Spawning a blank character");

            //Spawn the rest of the crowd
            GameObject blankObject = GameObject.Instantiate(blankNPCPrefab);
            var npcScript = blankObject.GetComponent<NPC>();

            blankObject.transform.SetParent(transform);

            blankObject.transform.Translate(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
