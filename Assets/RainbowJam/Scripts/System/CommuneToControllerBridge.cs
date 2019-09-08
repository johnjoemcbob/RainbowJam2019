using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuneToControllerBridge : MonoBehaviour
{
	public static CommuneToControllerBridge Instance;

	public int[] MaxPeopleBuildingUpgradeStages = new int[] { 5, 10, 15 };

	[Header( "References" )]
	[SerializeField]
	private GameObject CommuneNPCPrefab;
	public SceneController sceneController;

	[HideInInspector]
	public static float CurrentMaxPeople;

	protected Vector3 InitialPlayerPos;

	public void RegisterSceneController(SceneController controller)
    {
        sceneController = controller;
    }

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
        if(CommuneNPCPrefab == null)
        {
            Debug.LogError("Commune NPC Prefab not set, ensure it is before continuing.");
        }

		InitialPlayerPos = Player_Commune.Instance.transform.position;

		CurrentMaxPeople = MaxPeopleBuildingUpgradeStages[0];
	}

    public void ResetPlayerPosition()
    {
		if ( Player_Commune.Instance != null )
		{
			Player_Commune.Instance.transform.position = InitialPlayerPos;
		}
	}

    public void SpawnNewFriends(List<PersonInfo> friends)
    {
        if(friends != null && friends.Count > 0)
        {
            if(CommuneNPCPrefab != null)
            {
                foreach(var friend in friends)
                {
                    GameObject friendObject = GameObject.Instantiate(CommuneNPCPrefab);
                    
                    var friendScript = friendObject.GetComponent<NPC_Commune>();
					//friendScript.Init( true );
					friendScript.isInitialised = true;
					friendScript.GenerateAppearanceFromData(friend);

                    // Spawning all the friends together in one big clump might be, uh, weird?
                    friendObject.transform.SetParent(gameObject.transform);
                    
                    // New friends appear at the sellbox?
                    GameObject friendSpawn = SellBox.Instance.gameObject;
                    friendObject.transform.localPosition = friendSpawn.transform.localPosition;
                }
            }
        }
		// Spawned, now empty for next time
		friends.Clear();
	}

    public void SummonDialogueBubble(string bubbleText)
    {
        sceneController.SummonDialogueBubble(bubbleText);
    }

	public static int GetCurrentCommuneNPCs()
	{
		// Find count of active npcs (those who leave are simply disabled gameobjects)
		return Instance.GetComponentsInChildren<NPC_Commune>().Length;
	}
}
