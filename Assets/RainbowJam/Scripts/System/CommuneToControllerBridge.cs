using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuneToControllerBridge : MonoBehaviour
{
    [SerializeField]
    private GameObject CommuneNPCPrefab;

    public SceneController sceneController;

	protected Vector3 InitialPlayerPos;

	public void RegisterSceneController(SceneController controller)
    {
        sceneController = controller;
    }

    void Start()
    {
        if(CommuneNPCPrefab == null)
        {
            Debug.LogError("Commune NPC Prefab not set, ensure it is before continuing.");
        }

		InitialPlayerPos = Player_Commune.Instance.transform.position;
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
					friendScript.Init( true );
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
}
