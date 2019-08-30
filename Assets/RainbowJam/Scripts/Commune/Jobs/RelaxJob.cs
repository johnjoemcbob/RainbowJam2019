using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class RelaxJob : Job
{
	protected Point TargetPos;

	public override bool IsAvailable( NPC_Commune npc )
	{
		return true;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		Duration = Random.Range( 5, 10 );
		TargetPos = NPC.CurrentPos; // Flag to randomise next update
	}

	public override void Update()
	{
		base.Update();

		// TODO try to find other people relaxing or go inside
		// TODO chug jam
		if ( NPC.CurrentPos == TargetPos )
		{
			// TODO regen if no path found (could try to path to impasse tile, bad)
			TargetPos = new Point( Random.Range( 0, BuildableArea.Instance.GridSquares ), Random.Range( 0, BuildableArea.Instance.GridSquares ) );
			NPC.SetTargetCell( TargetPos );
		}
	}

	public override void Finish()
	{
		base.Finish();
	}
}
