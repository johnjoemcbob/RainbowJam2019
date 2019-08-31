using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    private GameObject CityScenePrefab;

    [SerializeField]
    private GameObject CommuneScenePrefab;

    private GameStates CurrentState = GameStates.INVALID;

    // Start is called before the first frame update
    void Start()
    {
        if(CityScenePrefab == null || CommuneScenePrefab == null)
        {
            Debug.LogError("ERROR: City Scene Prefab or Commune Scene Prefab have not been set!");
        }
        else
        {
            RequestStateChange(GameStates.COMMUNE);
        }
    }

    public void RequestStateChange(GameStates newState)
    {
        if(newState != CurrentState)
        {
            switch(newState)
            {
                case GameStates.CITY:

                    // Going to the city. 
                    Debug.Log("Switching to City scene!");
                    
                    // Disable other scenes.
                    CommuneScenePrefab.SetActive(false);
                    
                    // Contact the city controller, and get it to set things up.
                    //var cityController = CityScenePrefab.GetComponent<CityController>();
                    // cityController.PlayerEnteredCity();
                    // cityController.ShuffleCrowd etc etc ?
                    

                    // Enable city scene.   
                    CityScenePrefab.SetActive(true);


                    break;

                case GameStates.COMMUNE:

                    // Going to the commune. 
                    Debug.Log("Switching to Commune scene!");
                    
                    // Disable other scenes.
                    CityScenePrefab.SetActive(false);
                    
                    // Contact the city controller, and get it to set things up.
                    //var communeController = CommuneScenePrefab.GetComponent<CommuneController>();
                    // communeController.PlayerReturnedFromCityWithFriends(List<PersonInfo> friends);
                    // communeController.SpawnNewFriends(); // or something
                    
                    // Enable city scene.
                    CommuneScenePrefab.SetActive(true);

                    break;
                default:
                    Debug.LogError("Requested change to invalid state... this is probably not what you wanted.");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Requested to change state to the state we are already in, ignoring.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
