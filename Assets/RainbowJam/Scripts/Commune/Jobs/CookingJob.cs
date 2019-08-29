using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingJob : Job
{
	protected Workstation Workstation;

	public override bool IsAvailable( NPC_Commune npc )
	{
		List<Workstation> workable = new List<Workstation>();
			foreach ( var station in GameObject.FindObjectsOfType<Workstation>() )
			{
				if ( station.Berries >= Workstation.BERRIES_TO_JAM && station.AssignedNPC == null )
				{
					workable.Add( station );
				}
			}
		if ( workable.Count > 0 )
		{
			Workstation = workable[Random.Range( 0, workable.Count )];
			Workstation.AssignedNPC = npc;
			return true;
		}
		return false;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		Duration = 0;

		// Path towards the station
		NPC.SetTargetCell( BuildableArea.GetCellFromPosition( Workstation.gameObject ) );
	}

	public override void Update()
	{
		base.Update();

		// Cook if reached
		if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( Workstation.gameObject ) )
		{
			NPC.SetTargetCell( NPC.CurrentPos ); // Why is this necessary?
			Workstation.Cook();
		}

		// Don't stand around if nothing can be made
		if ( Workstation.Berries < Workstation.BERRIES_TO_JAM )
		{
			Debug.Log( "ran out of berries" );
			Workstation.AssignedNPC = null;
			Finish();
		}
	}

	public override void Finish()
	{
		base.Finish();
	}
}
