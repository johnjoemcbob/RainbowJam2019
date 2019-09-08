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
        // TODO: Reset/respawn crowds & player pos?

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

}
