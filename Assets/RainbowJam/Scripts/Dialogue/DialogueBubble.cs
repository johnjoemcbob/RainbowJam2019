using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBubble : MonoBehaviour
{
    public string TargetString;

    [SerializeField]
    private AudioSource TextTickSound;
    [SerializeField]
    private AudioSource OpenCloseSound;

    [HideInInspector]
    public string CurrentString;

    private float TextAddFrameDelay = 1.0f;
    private float FrameDelayAccumulator = 0.0f;

    [HideInInspector]
    public bool FinishedDisplaying = false;

    [SerializeField]
    private Text TextElement;
	public Text TitleElement;
	public GameObject MouseIcon;

	[SerializeField]
    private RectTransform BackdropElement;

	private float DisappearStart = 200;
	private Vector2 BubbleMinHelper = new Vector2( 1, 1 );
	private Vector2 BubbleMaxHelper = new Vector2( -1, -1 );
	private float BubbleScale;

    [HideInInspector]
    public bool Opening = true;
    public bool DidPlayOpeningSound = false;

    [HideInInspector]
    public bool Closing = false;
    public bool DidPlayClosingSound = false;
    
    // Helper function & variables.
    public static List<DialogueBubble> QueuedDialogues = new List<DialogueBubble>();
    
    public static void SummonDialogueBubble(string targetText, string title, GameObject dialogueBubblePrefab, Transform canvas)
    {
        // Create and queue a dialogue bubble.
        var newBubble = GameObject.Instantiate(dialogueBubblePrefab);
        newBubble.transform.SetParent(canvas);

        var bubbleScript = newBubble.GetComponent<DialogueBubble>();
        bubbleScript.TargetString = targetText;
		bubbleScript.TitleElement.enabled = ( title != "" );
		bubbleScript.TitleElement.text = title;
		bubbleScript.Initialise();

        QueuedDialogues.Add(bubbleScript);

        if(QueuedDialogues.Count > 1)
        {
            newBubble.SetActive(false);
        }
    }

    void Start()
    {
        if(TextElement == null || BackdropElement == null)
        {
            Debug.LogError("Text or backdrop not set in prefab. Dead!");
        }
    }

    void Initialise()
    {
        TextElement.text = "";
        BubbleScale = 0.0f; // Scale bubble up
	}

    void Update()
    {
		MouseIcon.SetActive( BubbleScale == 1 );

        if(BubbleScale < 1.0f && Opening)
        {
            if(!DidPlayOpeningSound)
            {
                OpenCloseSound.Play();
                DidPlayOpeningSound = true;
            }
            BubbleScale += 0.1f * (Time.deltaTime * 60);

            BubbleScale = Mathf.Clamp01(BubbleScale);

			BackdropElement.offsetMin = BubbleMinHelper * DisappearStart * ( 1 - BubbleScale );
			BackdropElement.offsetMax = BubbleMaxHelper * DisappearStart * ( 1 - BubbleScale );

            if(BubbleScale >= 1.0f)
            {
                Opening = false;
            }
        }
        else if(BubbleScale > 0.0f && Closing)
        {
            if(!DidPlayClosingSound)
            {
                OpenCloseSound.Play();
                DidPlayClosingSound = true;
            }
            BubbleScale -= 0.1f * (Time.deltaTime * 60);

            BubbleScale = Mathf.Clamp01(BubbleScale);

			BackdropElement.offsetMin = BubbleMinHelper * DisappearStart * ( 1 - BubbleScale );
			BackdropElement.offsetMax = BubbleMaxHelper * DisappearStart * ( 1 - BubbleScale );

            CurrentString = ""; // this used to reverse-display the characters while the bubble shrank, but it's not super easy to predict when long strings will get squished and suddenly expand upwards for a frame or two, so Zoip. gone. done.
            TextElement.text = CurrentString;

            if(BubbleScale <= 0.0f)
            {
                Closing = false;

                // Remove from queue, and advance the queue if there is more to show.
                if(QueuedDialogues.Contains(this))
                {
                    QueuedDialogues.Remove(this);

                    if(QueuedDialogues.Count > 0)
                    {
                        QueuedDialogues[0].gameObject.SetActive(true);
                    }
                }

                // Die, bubble!
                GameObject.Destroy(gameObject);
            }
        }

        if(!FinishedDisplaying && !Closing)
        {
            FrameDelayAccumulator += Time.deltaTime;

            // Fixed-timestep character advancement. Should add characters at a constant rate.
            while(FrameDelayAccumulator > ((float)TextAddFrameDelay/60))
            {
                FrameDelayAccumulator -= ((float)TextAddFrameDelay/60);
                AdvanceTextDisplay();
            }
        }

		if ( Input.GetMouseButtonDown( 0 ) )
		{
			if (FinishedDisplaying)
			{
                Dismiss();
            }
			else if ( !FinishedDisplaying )
			{
				CurrentString = TargetString;
				TextElement.text = CurrentString;
				FinishedDisplaying = true;
			}
        }
    }

    public void Dismiss()
    {
        FinishedDisplaying = false;
        Opening = false;
        Closing = true;
    }

    public void AdvanceTextDisplay()
    {
        // Advance by one frame.
        if(!FinishedDisplaying)
        {
            if(CurrentString == TargetString)
            {
                FinishedDisplaying = true;
            }
            else
            {
                CurrentString += TargetString[CurrentString.Length];
                TextElement.text = CurrentString;

                if(CurrentString.Length % 3 == 0)
                {
                    TextTickSound.pitch = Random.Range(0.85f, 1.15f);
                    TextTickSound.Play();
                }
            }
        }
    }

    public void SetTargetTextAndWipe(string newTargetText)
    {
        TargetString = newTargetText;
        CurrentString = "";

        FinishedDisplaying = false;
        TextElement.text = CurrentString;
    }
}
