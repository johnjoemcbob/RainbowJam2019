using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringJob : Job
{
	protected SoilBush Bush;
	protected Workstation DropOff;

	public override bool IsAvailable( NPC_Commune npc )
	{
		List<SoilBush> gatherable = new List<SoilBush>();
			foreach ( var bush in GameObject.FindObjectsOfType<SoilBush>() )
			{
				if ( bush.CurrentStage == SoilBush.Stage.Harvestable && bush.AssignedNPC == null )
				{
					gatherable.Add( bush );
				}
			}
		if ( gatherable.Count > 0 )
		{
			Bush = gatherable[Random.Range( 0, gatherable.Count )];
			Bush.AssignedNPC = npc;
			return true;
		}
		return false;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		Duration = 0;

		// Path towards the bush
		NPC.SetTargetCell( BuildableArea.GetCellFromPosition( Bush.gameObject ) );
	}

	public override void Update()
	{
		base.Update();

		// Harvest if reach
		if ( NPC.Berries == 0 )
		{
			if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( Bush.gameObject ) )
			{
				NPC.Berries = Bush.Harvest();
				Bush.AssignedNPC = null;
			}
		}
		else
		{
			if ( DropOff == null )
			{
				FindWorkstation();
			}
			else
			{
				NPC.SetIfNotTargetCell( BuildableArea.GetCellFromPosition( DropOff.DropZone ) );
				if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( DropOff.DropZone ) )
				{
					// Remove number of berries accepted and repeat until hands empty
					NPC.Berries = DropOff.AddBerry( NPC.Berries );
					if ( NPC.Berries != 0 )
					{
						DropOff = null;
					}
					else
					{
						Finish();

						NPC.TaskComplete();
					}
				}
			}
		}
	}

	public override void Finish()
	{
		base.Finish();

		NPC.Berries = 0;
	}

	protected void FindWorkstation()
	{
		// Find workstation with space for berries (lowest?)
		// TODO prioritise top up of currently worked stations?
		foreach ( var workstation in GameObject.FindObjectsOfType<Workstation>() )
		{
			if ( workstation.Berries < Workstation.MAX_BERRIES )
			{
				DropOff = workstation;
				break;
			}
		}

		if ( DropOff != null )
		{
			NPC.SetTargetCell( BuildableArea.GetCellFromPosition( DropOff.DropZone ) );
			// If find then set duration back to 0
			Duration = 0;
		}
		{
			// If don't find one then set duration timeout, but still search every update
			if ( Duration == 0 )
			{
				StartTime = Time.time;
				Duration = 5; // TODO make this clearer variable
			}
		}

		// At end of timeout any fruit is destroyed
		// TODO If it takes too long drop berries in sell bin?
		// TODO Or just eat it?
	}
}
