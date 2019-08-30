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
				if ( station.Berries >= Workstation.BERRIES_TO_JAM && station.Jams < Workstation.MAX_JAMS && station.AssignedNPC == null )
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
		NPC.SetTargetCell( BuildableArea.GetCellFromPosition( Workstation.CookZone ) );
	}

	public override void Update()
	{
		base.Update();

		// Cook if reached
		if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( Workstation.CookZone ) )
		{
			NPC.SetTargetCell( NPC.CurrentPos ); // Safety, left over from bad debug
			if ( Workstation.Cook() )
			{
				// Try to finish after each batch, but if wants to do more then may just be reassigned
				Workstation.AssignedNPC = null;
				Finish();

				NPC.TaskComplete();
			}
		}

		// Don't stand around if nothing can be made
		if ( Workstation.Berries < Workstation.BERRIES_TO_JAM || Workstation.Jams >= Workstation.MAX_JAMS )
		{
			Workstation.AssignedNPC = null;
			Finish();
		}
	}

	public override void Finish()
	{
		base.Finish();
	}
}
