using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    PersonInfo personInfo;

    [SerializeField]
    private GameObject spawnPointContainer;

    [SerializeField]
    private GameObject friendNPCPrefab;

    [SerializeField]
    private GameObject blankNPCPrefab;

    public int friendPoolSize = 5;
    public int totalCrowdSize = 100;

    float yOffset = 0.25f;
    List<Vector3> spawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        //Sort out spawn positions
        spawnPoints = new List<Vector3>();
        Vector3 spawnPos = Vector3.zero;

        if (spawnPointContainer != null)
        {
            int spawnPointCount = spawnPointContainer.transform.childCount;

            if (spawnPointCount > 0)
            {
                for (int i = 0; i < spawnPointCount; i++)
                {
                    spawnPoints.Add(new Vector3(spawnPointContainer.transform.GetChild(i).transform.position.x, yOffset, spawnPointContainer.transform.GetChild(i).transform.position.z));
                }
            }
        }



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

                //Random spawn point
                if (spawnPoints.Count > 0)
                {
                    int spawnPt = Random.Range(0, spawnPoints.Count - 1);
                    friendObject.transform.position = spawnPoints[spawnPt];
                }
                else
                {
                    friendObject.transform.Translate(0, yOffset, 0);
                }
            }
        }


        //[TODO] Spawn false positives here


        //Spawn the remaining crowd as blank
        int remainingCrowd = totalCrowdSize - friendPoolSize;

        for (int i = 0; i < remainingCrowd; i++)
        {
            //Spawn the rest of the crowd
            GameObject blankObject = GameObject.Instantiate(blankNPCPrefab);
            var npcScript = blankObject.GetComponent<NPC>();

            blankObject.transform.SetParent(transform);

            //Random spawn point
            if (spawnPoints.Count > 0)
            {
                int spawnPt = Random.Range(0, spawnPoints.Count - 1);
                blankObject.transform.position = spawnPoints[spawnPt];
            }
            else
            {
                blankObject.transform.Translate(0, yOffset, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
