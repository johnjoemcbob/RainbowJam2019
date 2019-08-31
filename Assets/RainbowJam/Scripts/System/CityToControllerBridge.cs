using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityToControllerBridge : MonoBehaviour
{
    private List<PersonInfo> friends;

    public void SetFriends(List<PersonInfo> newFriends)
    {
        friends = newFriends;
    }

    public List<PersonInfo> GetFriends()
    {
        return friends;
    }

}
