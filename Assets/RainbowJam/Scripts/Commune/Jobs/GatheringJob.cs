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

		// Plant if reach
		if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( Bush.gameObject ) )
		{
			NPC.Berries = Bush.Harvest();
			Bush.AssignedNPC = null;
		}

		if ( NPC.Berries > 0 )
		{
			if ( DropOff == null )
			{
				FindWorkstation();
			}
			else if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( DropOff.gameObject ) )
			{
				// Remove number of berries accepted and repeat until hands empty
				NPC.Berries = DropOff.AddBerry( NPC.Berries );
				if ( NPC.Berries != 0 )
				{
					DropOff = null;
				}
			}
		}

		// If plant is harvested and NPC has dropped off all berries then continue to next job
		if ( Bush.CurrentStage != SoilBush.Stage.Harvestable && NPC.Berries == 0 )
		{
			Finish();
		}
	}

	public override void Finish()
	{
		base.Finish();
	}

	protected void FindWorkstation()
	{
		// TODO part of this job is bringing this harvested berry to a workstation
		// Find workstation with space for berries (lowest?)
		// TODO prioritise top up of currently worked stations?

		DropOff = GameObject.FindObjectOfType<Workstation>();
		if ( DropOff.Berries >= Workstation.MAX_BERRIES )
		{
			DropOff = null;
			Duration = 5; // TODO make this clearer variable
		}
		if ( DropOff != null )
		{
			NPC.SetTargetCell( BuildableArea.GetCellFromPosition( DropOff.gameObject ) );
			Duration = 0;
		}

		// If don't find one then set duration timeout, but still search every update
		// If find then set duration back to 0
		// At end of timeout any fruit is destroyed
		// TODO If it takes too long drop berries in sell bin?
		// TODO Or just eat it?
	}
}
