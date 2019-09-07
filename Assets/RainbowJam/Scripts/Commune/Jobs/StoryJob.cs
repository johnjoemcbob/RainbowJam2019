using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class StoryJob : Job
{
	protected Point TargetPos;

	public override bool IsAvailable( NPC_Commune npc )
	{
		// TODO if has next story segment available and player isn't already being talked at
		return false;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		// Find location next to player
		//Duration = Random.Range( 5, 10 );
		//TargetPos = NPC.CurrentPos; // Flag to randomise next update
	}

	public override void Update()
	{
		base.Update();

		if ( NPC.CurrentPos == TargetPos )
		{
			// Reached player
			// Open dialogue
			// Mark story as told at this point
		}
	}

	public override void Finish()
	{
		base.Finish();
	}
}
