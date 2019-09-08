using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
	public static CrowdManager Instance;

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

	[HideInInspector]
	public List<GameObject> Friends = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> NotFriends = new List<GameObject>();

    private void Awake()
	{
		Instance = this;
        spawnPoints = new List<Vector3>();
    }

    void Start()
    {
        //RefreshCrowd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetSpawnPoints()
    {
        //Sort out spawn positions
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
    }

    public void RefreshCrowd()
    {
        if (spawnPoints.Count <= 0)
            GetSpawnPoints();


        //CLEAR EVERYTHING
        GameObject currentNPCObj = null;

        //FRIEND LIST
        for (int i = 0; i < Friends.Count; i++)
        {
            currentNPCObj = Friends[i];
            Destroy(currentNPCObj);
        }

        Friends.Clear();

        //NOT FRIEND LIST
        for (int i = 0; i < NotFriends.Count; i++)
        {
            currentNPCObj = NotFriends[i];
            Destroy(currentNPCObj);
        }

        NotFriends.Clear();



        //Spawn friends first
        for (int i = 0; i < friendPoolSize; i++)
        {
            //Spawn friends
            if (friendNPCPrefab != null)
            {
                GameObject friendObject = GameObject.Instantiate(friendNPCPrefab);
                var npcScript = friendObject.GetComponent<NPC>();
                npcScript.Init(true); //Flag these
                //npcScript.GenerateAppearanceFromData(PersonInfo.GenerateRandom("DEBUG_FRIEND"));

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

                Friends.Add(friendObject);
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

            NotFriends.Add(blankObject);
        }
    }
}
