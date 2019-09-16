using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTravelZone : MonoBehaviour
{
	public GameStates GoTo = GameStates.CITY;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.ToLower().Contains("player"))
        {
            SceneController.Instance.RequestStateChange(GoTo, false);
        }
    }
}
