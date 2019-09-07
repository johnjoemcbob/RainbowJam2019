using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTravelZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.ToLower().Contains("player"))
        {
            SceneController.Instance.RequestStateChange(GameStates.CITY, false);
        }
    }
}
