using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class StoryJob : Job
{
	protected Point TargetPos;

	public override bool IsAvailable( NPC_Commune npc )
	{
		return ( npc.GetPersonalStory().GetWantsToTalk() && ( Player_Commune.Instance.BeingTalkedAt == null ) );
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		// Flag so only one npc can talk at a time
		Player_Commune.Instance.BeingTalkedAt = npc;
	}

	public override void Update()
	{
		base.Update();

		// Path to player but stop when one square away
		int dist = Mathf.Abs( NPC.CurrentPos.x - TargetPos.x ) + Mathf.Abs( NPC.CurrentPos.y - TargetPos.y );
		if ( dist <= 2 )
		{
			// Stop moving
			NPC.SetTargetCell( NPC.CurrentPos );

			// Open dialogue
			// Mark story as told at this point
			NPC.TalkToPlayer();

			Finish();
		}
		else
		{
			// Continuously find player pos
			TargetPos = BuildableArea.GetCellFromPosition( Player_Commune.Instance.transform.position );
			NPC.SetIfNotTargetCell( TargetPos );
		}
	}

	public override void Finish()
	{
		base.Finish();

		Player_Commune.Instance.BeingTalkedAt = null;
	}
}
