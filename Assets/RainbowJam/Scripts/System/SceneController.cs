using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    private GameObject CityScene;

    [SerializeField]
    private GameObject CommuneScene;

    [SerializeField]
    private GameObject CommuneNPCPrefab;

    private GameStates CurrentState = GameStates.INVALID;

    // Start is called before the first frame update
    void Start()
    {
        if(CityScene == null || CommuneScene == null)
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
                    CommuneScene.SetActive(false);
                    
                    // Contact the city controller, and get it to set things up.
                    //var cityController = CityScenePrefab.GetComponent<CityController>();
                    // cityController.PlayerEnteredCity();
                    // cityController.ShuffleCrowd etc etc ?
                    

                    // Enable city scene.   
                    CityScene.SetActive(true);


                    break;

                case GameStates.COMMUNE:

                    // Going to the commune. 
                    Debug.Log("Switching to Commune scene!");
                    
                    // Disable other scenes.
                    CityScene.SetActive(false);

                    // Enable commune scene.
                    CommuneScene.SetActive(true);
                    
                    // Fetch gathered friends from city!
                    var cityBridge = CityScene.GetComponentInChildren<CityToControllerBridge>();

                    if(cityBridge != null)
                    {
                        SpawnNewFriends(cityBridge.GetFriends());
                    }

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

    // TODO: Move these to their own classes
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
                friendObject.transform.SetParent(CommuneScene.transform);
                
                GameObject friendSpawn = SellBox.Instance.gameObject;
                friendObject.transform.localPosition = friendSpawn.transform.localPosition;

            }
        }
        
    }
}
