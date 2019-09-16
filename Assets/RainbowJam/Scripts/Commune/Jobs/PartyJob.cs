using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class PartyJob : Job
{
	protected Point TargetPos;

	public override bool IsAvailable( NPC_Commune npc )
	{
		// If party, must party
		return CommuneToControllerBridge.Instance.IsParty;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		// TODO wander around until near other people? or something?
		NPC.SetPos( new Point( BuildableArea.Instance.GridSquares / 8 * 5 + Random.Range( 0, BuildableArea.Instance.GridSquares / 4 ), BuildableArea.Instance.GridSquares / 2 + Random.Range( 0, BuildableArea.Instance.GridSquares / 4 ) ) );
		TargetPos = NPC.CurrentPos; // Flag to randomise next update
	}

	public override void Update()
	{
		base.Update();

		// Just wander
		if ( NPC.CurrentPos == TargetPos )
		{
			TargetPos = new Point( BuildableArea.Instance.GridSquares / 8 * 5 + Random.Range( 0, BuildableArea.Instance.GridSquares / 4 ), BuildableArea.Instance.GridSquares / 2 + Random.Range( 0, BuildableArea.Instance.GridSquares / 4 ) );
			NPC.SetTargetCell( TargetPos );
		}

		// Path to player but stop when one square away
		//int dist = Mathf.Abs( NPC.CurrentPos.x - TargetPos.x ) + Mathf.Abs( NPC.CurrentPos.y - TargetPos.y );
		//if ( dist <= 2 )
		//{
		//	// Stop moving
		//	NPC.SetTargetCell( NPC.CurrentPos );

		//	// Open dialogue
		//	// Mark story as told at this point
		//	NPC.TalkToPlayer();

		//	Finish();
		//}
		//else
		//{
		//	// Continuously find player pos
		//	TargetPos = BuildableArea.GetCellFromPosition( Player_Commune.Instance.transform.position );
		//	NPC.SetIfNotTargetCell( TargetPos );
		//}
	}

	public override void Finish()
	{
		base.Finish();
	}
}
