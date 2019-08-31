using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellBox : MonoBehaviour
{
	public static SellBox Instance;

	public const int Price = 5;

	[Header( "Variables" )]
	public int[] StoryMilestones;

	[Header( "References" )]
	public Text MoneyText;
	public GameObject DropZone;

	[HideInInspector]
	public int Money = 0;

	[SerializeField]
	private RectTransform ThermometerTop;

	[SerializeField]
	private RectTransform ThermometerScale;

	protected int CurrentMilestone = 0;

	private void Awake()
	{
		Instance = this;

		if(ThermometerScale == null || ThermometerTop == null)
		{
			Debug.LogError("Thermometer sprites haven't been set in the prefab. Oops!!");
		}
		else
		{
			SetThermometerValue(0.0f);
		}
	}

	void Start()
	{
		// Ensure data grid is filled properly (in case added in editor)
		var cell = BuildableArea.GetCellFromPosition( gameObject );
		BuildableArea.Instance.Grid.nodes[cell.x, cell.y].Type = NesScripts.Controls.PathFind.NodeContent.Impasse;
		BuildableArea.Instance.Grid.nodes[cell.x, cell.y].walkable = false;
	}

	/// 0.0 - 1.0
	void SetThermometerValue(float value)
	{
		value = Mathf.Clamp(value, 0.0f, 1.0f);

		// Magic-numbering the hell out of this, woop woop
		float newYPos = (100) + (495 * value);
		ThermometerTop.localPosition = new Vector3(ThermometerTop.localPosition.x, newYPos, ThermometerTop.localPosition.z);
	}

    void Update()
    {
		// Make sure the thermometer scaling part meets the bottom of the thermometer top piece.
		ThermometerScale.offsetMax = new Vector2(ThermometerScale.offsetMax.x, ThermometerTop.offsetMin.y);
    }

	public void AddMoney( int add )
	{
		Money += add;
		MoneyText.text = "$" + Money.ToString();

		// Check for next milestone reached
		if ( CurrentMilestone < StoryMilestones.Length - 1 && Money >= StoryMilestones[CurrentMilestone+1] )
		{
			House.Instance.AddStory();

			if(CurrentMilestone+1 < StoryMilestones.Length)
			{
				CurrentMilestone++;
				SetThermometerValue(0.0f);
			}
			else
			{
				SetThermometerValue(1.0f);
			}
		}
		else
		{
			SetThermometerValue( ((float)Money - (float)StoryMilestones[CurrentMilestone]) / ((float)StoryMilestones[CurrentMilestone+1] - (float)StoryMilestones[CurrentMilestone]) );
		}
	}
	
	public void SellJam( int count )
	{
		AddMoney( count * Price );
	}
}
