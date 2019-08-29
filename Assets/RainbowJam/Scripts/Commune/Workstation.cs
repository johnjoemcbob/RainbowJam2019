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

	[HideInInspector]
	public NPC_Commune AssignedNPC;

	protected float CookStartTime = 0;

	private void Start()
	{
		UpdateResources();
	}

	// Return any that could not be accepted
	public int AddBerry( int count )
	{
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

	public void Cook()
	{
		if ( Berries < BERRIES_TO_JAM ) return;
		if ( Jams >= MAX_JAMS ) return;

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
		}
	}

	protected void UpdateResources()
	{
		UpdateResourceDisplay();
	}

	protected void UpdateResourceDisplay()
	{
		foreach ( Transform child in BerriesParent )
		{
			child.gameObject.SetActive( child.GetSiblingIndex() < Berries );
		}
		foreach ( Transform child in JamsParent )
		{
			child.gameObject.SetActive( child.GetSiblingIndex() < Jams );
		}
	}
}
