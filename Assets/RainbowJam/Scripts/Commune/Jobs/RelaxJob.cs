using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxJob : Job
{
	public override bool IsAvailable( NPC_Commune npc )
	{
		return true;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		Duration = Random.Range( 0.5f, 5 );
	}

	public override void Update()
	{
		base.Update();

		// TODO try to find other people relaxing or go inside
		// TODO chug jam
	}

	public override void Finish()
	{
		base.Finish();
	}
}
