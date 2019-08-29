using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingJob : Job
{
	protected SoilBush SoilBush;

	public override bool IsAvailable( NPC_Commune npc )
	{
		List<SoilBush> Plantable = new List<SoilBush>();
			foreach ( var tilledsoil in GameObject.FindObjectsOfType<SoilBush>() )
			{
				if ( tilledsoil.CurrentStage == SoilBush.Stage.Tilled && tilledsoil.AssignedNPC == null )
				{
					Plantable.Add( tilledsoil );
				}
			}
		if ( Plantable.Count > 0 )
		{
			SoilBush = Plantable[Random.Range( 0, Plantable.Count )];
			Debug.Log( SoilBush.GetInstanceID() + ", " + SoilBush.AssignedNPC + ", " + Time.time );
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
		if ( NPC.Current == BuildableArea.GetCellFromPosition( SoilBush.gameObject ) )
		{
			SoilBush.Plant();
			SoilBush.AssignedNPC = null;
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

		//SoilBush.AssignedNPC = null;
	}
}
