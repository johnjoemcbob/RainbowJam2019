using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingJob : Job
{
	protected Workstation Workstation;

	public override bool IsAvailable( NPC_Commune npc )
	{
		List<Workstation> workable = new List<Workstation>();
			foreach ( var station in GameObject.FindObjectsOfType<Workstation>() )
			{
				if ( station.Jams > 0 )
				{
					workable.Add( station );
				}
			}
		if ( workable.Count > 0 )
		{
			Workstation = workable[Random.Range( 0, workable.Count )];
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

		if ( NPC.Jams == 0 )
		{
			// Pickup jam if reached
			if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( Workstation.gameObject ) )
			{
				NPC.SetTargetCell( BuildableArea.GetCellFromPosition( SellBox.Instance.gameObject ) );

				NPC.Jams = 1;
				Workstation.AddJam( -1 );
			}
		}
		else
		{
			if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( SellBox.Instance.gameObject ) )
			{
				SellBox.Instance.SellJam( NPC.Jams );
				NPC.Jams = 0;
				NPC.TaskComplete();

				Finish();
			}
		}

		// Someone else took the jam?
		if ( Workstation.Jams == 0 && NPC.Jams == 0 )
		{
			Finish();
		}
	}

	public override void Finish()
	{
		base.Finish();
	}
}
