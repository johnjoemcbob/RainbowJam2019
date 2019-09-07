using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    private GameObject CityScene;
    private CityToControllerBridge CityBridge;

    [SerializeField]
    private GameObject CommuneScene;
    private CommuneToControllerBridge CommuneBridge;

    [SerializeField]
    private Canvas DialogueBubbleCanvas;
    [SerializeField]
    private GameObject DialogueBubblePrefab;

    public static SceneController Instance;


    private GameStates CurrentState = GameStates.INVALID;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if(CityScene == null || CommuneScene == null)
        {
            Debug.LogError("ERROR: City Scene Prefab or Commune Scene Prefab have not been set!");
        }
        else
        {
            CityBridge = CityScene.GetComponentInChildren<CityToControllerBridge>();
            CommuneBridge = CommuneScene.GetComponentInChildren<CommuneToControllerBridge>();
            CityBridge.RegisterSceneController(this);
            CommuneBridge.RegisterSceneController(this);
            RequestStateChange(GameStates.COMMUNE, true);
        }

        if(DialogueBubbleCanvas == null || DialogueBubblePrefab == null)
        {
            Debug.LogError("Dialogue bubble setup in main scene not correct. Please fix.");
        }
    }

    public void RequestStateChange(GameStates newState, bool isFirstTime)
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

                    SummonDialogueBubble("Travelling to the city...");
                    

                    // Enable city scene.   
                    CityScene.SetActive(true);

                    // Contact the city controller, and get it to set things up.
                    CityBridge.PlayerEnteredCity();


                    break;

                case GameStates.COMMUNE:

                    // Going to the commune. 
                    Debug.Log("Switching to Commune scene!");
                    
                    // Disable other scenes.
                    CityScene.SetActive(false);

                    if(!isFirstTime)
                    {
                        SummonDialogueBubble("Returning home...");
                    }
                    else
                    {
                        SummonDialogueBubble("Welcome to the communal jam farm! Click to advance text.");
                        SummonDialogueBubble("Press the W, S, A, D keys to move around.");
                    }

                    // Enable commune scene.
                    CommuneScene.SetActive(true);
                    
                    // Reset Player Pos in Commune (so that they don't immediately re-enter the city.)
                    CommuneBridge.ResetPlayerPosition();

                    // Fetch gathered friends from city!
                    CommuneBridge.SpawnNewFriends(CityBridge.GetFriends());
                    

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

    // Quick and fast way for other scripts to yell at the player?
    public void SummonDialogueBubble(string bubbleText)
    {
        DialogueBubble.SummonDialogueBubble(bubbleText, DialogueBubblePrefab, DialogueBubbleCanvas.transform, new Vector2(Screen.width/6, 0.0f), new Vector2((-Screen.width/6), -Screen.height/4));
    }

  
}
