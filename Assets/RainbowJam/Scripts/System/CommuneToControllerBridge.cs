﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuneToControllerBridge : MonoBehaviour
{
	public static CommuneToControllerBridge Instance;
	public bool IsParty = false; // Different logic for party segment

	public int[] MaxPeopleBuildingUpgradeStages = new int[] { 5, 10, 15 };

	[Header( "References" )]
	[SerializeField]
	private GameObject CommuneNPCPrefab;
	public SceneController sceneController;

	[HideInInspector]
	public float CurrentMaxPeople;

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

	private void Update()
	{
		if ( Input.GetKey(KeyCode.F11 ) )
		{
			StartParty();
		}
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

	public void StartParty()
	{
		if ( IsParty ) return; // In case hold F11

		IsParty = true;

		// Enable party prefab
		transform.Find( "PartyMode" ).gameObject.SetActive( true );

		// Show dialogue popup
		SceneController.Instance.SummonDialogueBubble( "Wow! All your friends threw you a surprise party to say thank you!! Click e on them to talk and catch up with what they've been up to!" );

		// Readd all npcs
		foreach ( var npc in transform.GetComponentsInChildren<NPC_Commune>( true ) )
		{
			npc.gameObject.SetActive( true );
			npc.SetPos( BuildableArea.GetCellFromPosition( Player_Commune.Instance.transform.position ) );
			npc.FindJob();
			npc.SetParty();
		}

		// Set to night time
		FindObjectOfType<Camera>().backgroundColor = new Color( 0.2f, 0.2f, 0.2f, 1 );
		FindObjectOfType<Light>().intensity = 0.2f;
		foreach ( var sprite in FindObjectsOfType<SpriteRenderer>() )
		{
			sprite.color = sprite.color * Color.gray;
		}
	}

	public static int GetCurrentCommuneNPCs()
	{
		// Find count of active npcs (those who leave are simply disabled gameobjects)
		return Instance.GetComponentsInChildren<NPC_Commune>().Length;
	}
}
