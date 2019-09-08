using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBubble : MonoBehaviour
{
    public string TargetString;

    [HideInInspector]
    public string CurrentString;

    private float TextAddFrameDelay = 1.0f;
    private float FrameDelayAccumulator = 0.0f;

    [HideInInspector]
    public bool FinishedDisplaying = false;

    [SerializeField]
    private TextMeshProUGUI TextElement; 

    [SerializeField]
    private RectTransform BackdropElement; 

    private Vector2 BubbleMax;
    private Vector2 BubbleMin;
    private float BubbleScale;

    [HideInInspector]
    public bool Opening = true;

    [HideInInspector]
    public bool Closing = false;
    

    // Helper function & variables.
    public static List<DialogueBubble> QueuedDialogues = new List<DialogueBubble>();
    
    public static void SummonDialogueBubble(string targetText, GameObject dialogueBubblePrefab, Transform canvas, Vector2 offsetMin, Vector2 offsetMax)
    {
        // Create and queue a dialogue bubble.
        var newBubble = GameObject.Instantiate(dialogueBubblePrefab);
        newBubble.transform.SetParent(canvas);

        var bubbleScript = newBubble.GetComponent<DialogueBubble>();
        bubbleScript.TargetString = targetText;
        bubbleScript.InitialisePositioning(offsetMin, offsetMax);

        QueuedDialogues.Add(bubbleScript);

        if(QueuedDialogues.Count > 1)
        {
            newBubble.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(TextElement == null || BackdropElement == null)
        {
            Debug.LogError("Text or backdrop not set in prefab. Dead!");
        }
        else
        {
            
        }
    }

    void InitialisePositioning(Vector2 offsetMin, Vector2 offsetMax)
    {
        BubbleMax = offsetMax;
        BubbleMin = offsetMin;

        TextElement.SetText("");
        BackdropElement.offsetMax = BubbleMax * 2.0f;
        BackdropElement.offsetMin = BubbleMin * 2.0f;
        BubbleScale = 0.0f; // scale bubble up?
    }

    // Update is called once per frame
    void Update()
    {
        if(BubbleScale < 1.0f && Opening)
        {
            BubbleScale += 0.1f * (Time.deltaTime * 60);

            BubbleScale = Mathf.Clamp01(BubbleScale);

            BackdropElement.offsetMin = BubbleMin * (2.0f - BubbleScale);
            BackdropElement.offsetMax = BubbleMax * (2.0f - BubbleScale);

            if(BubbleScale >= 1.0f)
            {
                Opening = false;
            }
        }
        else if(BubbleScale > 0.0f && Closing)
        {
            BubbleScale -= 0.1f * (Time.deltaTime * 60);

            BubbleScale = Mathf.Clamp01(BubbleScale);

            BackdropElement.offsetMin = BubbleMin * (2.0f - BubbleScale);
            BackdropElement.offsetMax = BubbleMax * (2.0f - BubbleScale);

            CurrentString = ""; // this used to reverse-display the characters while the bubble shrank, but it's not super easy to predict when long strings will get squished and suddenly expand upwards for a frame or two, so Zoip. gone. done.
            TextElement.SetText(CurrentString);

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

        if(FinishedDisplaying)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Dismiss();
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
                TextElement.SetText(CurrentString);
            }
        }
    }

    public void SetTargetTextAndWipe(string newTargetText)
    {
        TargetString = newTargetText;
        CurrentString = "";

        FinishedDisplaying = false;
        TextElement.SetText(CurrentString);
    }
}
