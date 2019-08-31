using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuneToControllerBridge : MonoBehaviour
{
    [SerializeField]
    private GameObject CommuneNPCPrefab;

    void Start()
    {
        if(CommuneNPCPrefab == null)
        {
            Debug.LogError("Commune NPC Prefab not set, ensure it is before continuing.");
        }
    }

    public void SpawnNewFriends(List<PersonInfo> friends)
    {
        if(CommuneNPCPrefab != null)
        {
            foreach(var friend in friends)
            {
                GameObject friendObject = GameObject.Instantiate(CommuneNPCPrefab);
                
                var friendScript = friendObject.GetComponent<NPC_Commune>();
                friendScript.GenerateAppearanceFromData(friend);

                // Spawning all the friends together in one big clump might be, uh, weird?
                friendObject.transform.SetParent(gameObject.transform);
                
                GameObject friendSpawn = SellBox.Instance.gameObject;
                friendObject.transform.localPosition = friendSpawn.transform.localPosition;

            }
        }
    }
}
