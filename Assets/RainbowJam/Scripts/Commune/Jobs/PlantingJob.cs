using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingJob : Job
{
	protected SoilBush SoilBush;

	public override bool IsAvailable( NPC_Commune npc )
	{
		List<SoilBush> plantable = new List<SoilBush>();
			foreach ( var tilledsoil in GameObject.FindObjectsOfType<SoilBush>() )
			{
				if ( tilledsoil.CurrentStage == SoilBush.Stage.Tilled && tilledsoil.AssignedNPC == null )
				{
					plantable.Add( tilledsoil );
				}
			}
		if ( plantable.Count > 0 )
		{
			SoilBush = plantable[Random.Range( 0, plantable.Count )];
			SoilBush.AssignedNPC = npc;
			return true;
		}
		return false;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		Duration = 0;

		// Path towards the soil
		NPC.SetTargetCell( BuildableArea.GetCellFromPosition( SoilBush.gameObject ) );
	}

	public override void Update()
	{
		base.Update();

		// Plant if reach
		if ( NPC.CurrentPos == BuildableArea.GetCellFromPosition( SoilBush.gameObject ) )
		{
			SoilBush.Plant();
			SoilBush.AssignedNPC = null;

			NPC.TaskComplete();
		}

		// If plant is planted by someone else also move on
		if ( SoilBush.CurrentStage != SoilBush.Stage.Tilled )
		{
			Finish();
		}
	}

	public override void Finish()
	{
		base.Finish();
	}
}
