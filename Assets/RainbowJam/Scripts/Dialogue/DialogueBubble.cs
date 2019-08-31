using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBubble : MonoBehaviour
{
    public string TargetString;

    [HideInInspector]
    public string CurrentString;

    public int TextAddFrameDelay = 3;

    [HideInInspector]
    public bool FinishedDisplaying = false;

    [SerializeField]
    private TextMeshProUGUI TextElement; 

    [SerializeField]
    private RectTransform BackdropElement; 

    private Vector2 BubbleMax;
    private Vector2 BubbleMin;
    private float BubbleScale;


    // Start is called before the first frame update
    void Start()
    {
        if(TextElement == null || BackdropElement == null)
        {
            Debug.LogError("Text or backdrop not set in prefab. Dead!");
        }
        else
        {
            TextElement.SetText("");
            BubbleMax = BackdropElement.offsetMax;
            BubbleMin = BackdropElement.offsetMin;
            BubbleScale = 0.1f; // scale bubble up?
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BubbleScale < 1.0f)
        {
            BubbleScale += 0.1f;

            BackdropElement.offsetMin = BubbleMin * BubbleScale;
            BackdropElement.offsetMax = BubbleMax * BubbleScale;
        }

        if(!FinishedDisplaying && Time.frameCount % TextAddFrameDelay == 0)
        {
            AdvanceTextDisplay();
        }
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
