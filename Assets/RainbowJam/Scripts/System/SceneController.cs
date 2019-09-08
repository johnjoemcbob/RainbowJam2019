using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	public static SceneController Instance;

	[SerializeField]
    private GameObject CityScene;
    public CityToControllerBridge CityBridge;

    [SerializeField]
    private GameObject CommuneScene;
	public CommuneToControllerBridge CommuneBridge;

    [SerializeField]
    private Canvas DialogueBubbleCanvas;
    [SerializeField]
    private GameObject DialogueBubblePrefab;

    [SerializeField]
    private UnityEngine.UI.Image ScreenFader;
    private float FadeDir = 1;
    private float FadeAmt = 1.0f;
    private bool Fading = false;

    private bool FirstTime = false;
    private Action OnFadeReachedPeak;


    private GameStates CurrentState = GameStates.INVALID;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
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

    public void SwitchToCity()
    {
        // Going to the city. 
        Debug.Log("Switching to City scene!");
        
        // Disable other scenes.
        CommuneScene.SetActive(false);

        SummonDialogueBubble("Travelling to the city...");
        

        // Enable city scene.   
        CityScene.SetActive(true);

        // Contact the city controller, and get it to set things up.
        CityBridge.PlayerEnteredCity();

        OnFadeReachedPeak -= SwitchToCity;
    }

	public void SwitchToCommune()
    {
        // Going to the commune. 
        Debug.Log("Switching to Commune scene!");
        
        // Disable other scenes.
        CityScene.SetActive(false);

        if(!FirstTime)
        {
            SummonDialogueBubble("Returning home...");
        }
        else
        {
            SummonDialogueBubble("Welcome to the communal jam farm! Click to advance text.");
            //SummonDialogueBubble("Press the W, S, A, D keys to move around.");
        }

		// Reset Player Pos in Commune (so that they don't immediately re-enter the city.)
		CommuneBridge.ResetPlayerPosition();

		// Enable commune scene.
		CommuneScene.SetActive(true);

        // Fetch gathered friends from city!
        CommuneBridge.SpawnNewFriends(CityBridge.GetFriends());

        FirstTime = false;

        OnFadeReachedPeak -= SwitchToCommune;
    }

    public void RequestStateChange(GameStates newState, bool isFirstTime)
    {
        FirstTime = isFirstTime;

        if(newState != CurrentState)
        {
            Fading = true;

            switch(newState)
            {
                case GameStates.CITY:

                    OnFadeReachedPeak += SwitchToCity;                    

                    break;

                case GameStates.COMMUNE:

                    OnFadeReachedPeak += SwitchToCommune;
                    
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
        if(Fading)
        {
            FadeAmt += 0.025f * FadeDir * (Time.deltaTime * 60);

            if(FadeAmt >= 1.0f && FadeDir == 1)
            {
                FadeDir = -1.0f;

                if(OnFadeReachedPeak != null)
                {
                    OnFadeReachedPeak();
                }
            }

            if(FadeAmt <= 0.0f && FadeDir == -1)
            {
                Fading = false;
                FadeDir = 1.0f;
            }

            ScreenFader.color = new Color(0, 0, 0, FadeAmt);
        }
    }

    // Quick and fast way for other scripts to yell at the player?
    public void SummonDialogueBubble(string bubbleText)
    {
        DialogueBubble.SummonDialogueBubble(bubbleText, DialogueBubblePrefab, DialogueBubbleCanvas.transform, new Vector2(Screen.width/4, 0.0f), new Vector2((-Screen.width/4), -Screen.height/2));
    }

  
}
