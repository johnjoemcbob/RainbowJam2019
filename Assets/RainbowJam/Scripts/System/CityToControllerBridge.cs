using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityToControllerBridge : MonoBehaviour
{
    private List<PersonInfo> friends = new List<PersonInfo>();

    public SceneController sceneController;

    public void RegisterSceneController(SceneController controller)
    {
        sceneController = controller;
    }

    public void PlayerEnteredCity()
    {
        // Reset/respawn crowds & player pos?

        GameObject crowdManager = transform.Find("CrowdManager").gameObject;

        if (crowdManager != null)
        {
            crowdManager.GetComponent<CrowdManager>().RefreshCrowd();
        }


        //Set commune variables
        GameObject collectionHandler = transform.Find("CollectionScript").gameObject;

        if (collectionHandler != null)
        {
            collectionHandler.GetComponent<CityCollectionHandler>().ResetNumbers();
            collectionHandler.GetComponent<CityCollectionHandler>().SetCommuneVariables(CommuneToControllerBridge.GetCurrentCommuneNPCs(), CommuneToControllerBridge.Instance.CurrentMaxPeople);
        }
    }

    public void SetFriends(List<PersonInfo> newFriends)
    {
        friends = newFriends;
    }

	public void AddFriend( PersonInfo newfriend )
	{
		friends.Add( newfriend );
	}

    public List<PersonInfo> GetFriends()
    {
        return friends;
    }

    public void SummonDialogueBubble(string bubbleText)
    {
        sceneController.SummonDialogueBubble(bubbleText);
    }

    public void SetCommuneVariables(int current, int max)
    {

    }

}
