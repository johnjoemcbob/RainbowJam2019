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

	protected int CurrentMilestone = 0;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		// Ensure data grid is filled properly (in case added in editor)
		var cell = BuildableArea.GetCellFromPosition( gameObject );
		BuildableArea.Instance.Grid.nodes[cell.x, cell.y].Type = NesScripts.Controls.PathFind.NodeContent.Impasse;
		BuildableArea.Instance.Grid.nodes[cell.x, cell.y].walkable = false;
	}

    void Update()
    {

    }

	public void AddMoney( int add )
	{
		Money += add;
		MoneyText.text = "$" + Money.ToString();

		// Check for next milestone reached
		if ( CurrentMilestone < StoryMilestones.Length - 1 && Money >= StoryMilestones[CurrentMilestone+1] )
		{
			House.Instance.AddStory();
			CurrentMilestone++;
		}
	}
	
	public void SellJam( int count )
	{
		AddMoney( count * Price );
	}
}
