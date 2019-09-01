using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour
{
	public const int MAX_BERRIES	= 20;
	public const int MAX_JAMS       = 10;
	public const int BERRIES_TO_JAM = 2;
	public const float COOK_TIME    = 2;

	[Header( "Variables" )]
	public int Berries = 0;
	public int Jams = 0;

	[Header( "References" )]
	public Transform BerriesParent;
	public Transform JamsParent;

	public GameObject RightSide;
	public GameObject DropZone;
	public GameObject CookZone;
	public GameObject PickupZone;

	[HideInInspector]
	public NPC_Commune AssignedNPC;
	private Berry.BerryType MostRecentBerryType;

	protected float CookStartTime = 0;

	private void Start()
	{
		UpdateResources();

		// Ensure data grid is filled properly (in case added in editor)
		{
			// Workstation impassable
			NesScripts.Controls.PathFind.Point cell;
			foreach ( var obj in new GameObject[] { gameObject, RightSide } )
			{
				cell = BuildableArea.GetCellFromPosition( obj );
				BuildableArea.Instance.Grid.nodes[cell.x, cell.y].Type = NesScripts.Controls.PathFind.NodeContent.Workstation;
				BuildableArea.Instance.Grid.nodes[cell.x, cell.y].walkable = false;
			}
		}
	}

	// Return any that could not be accepted
	public int AddBerry( int count, Berry.BerryType type )
	{
		MostRecentBerryType = type;
		int add = Mathf.Min( count, MAX_BERRIES - Berries );
		Berries += add;
		UpdateResources();
		return count - add;
	}

	public int AddJam( int count )
	{
		// TODO account for max
		Jams += count;
		UpdateResources();
		return count;
	}

	public bool Cook()
	{
		if ( Berries < BERRIES_TO_JAM ) return false;
		if ( Jams >= MAX_JAMS ) return false;

		if ( CookStartTime == 0 )
		{
			CookStartTime = Time.time;
			Debug.Log( "start cook" );
		}
		else if ( Time.time - CookStartTime >= COOK_TIME )
		{
			// Cooking process complete!
			Berries -= BERRIES_TO_JAM;
			Jams++;
			UpdateResources();

			CookStartTime = 0;
			Debug.Log( "Stop cook" );

			return true; // Just cooked
		}

		return false;
	}

	protected void UpdateResources()
	{
		UpdateResourceDisplay();
	}

	protected void UpdateResourceDisplay()
	{
		foreach ( Transform child in BerriesParent )
		{
			// Quick hack to make new berries on the table look like the last ones the NPC delivered.
			bool showingNewBerry = child.GetSiblingIndex() < Berries && !child.gameObject.activeInHierarchy;
			if(showingNewBerry)
			{
				child.gameObject.GetComponent<Berry>().InitWithType(MostRecentBerryType);
			}

			child.gameObject.SetActive( child.GetSiblingIndex() < Berries );
		}
		foreach ( Transform child in JamsParent )
		{
			child.gameObject.SetActive( child.GetSiblingIndex() < Jams );
		}
	}
}
