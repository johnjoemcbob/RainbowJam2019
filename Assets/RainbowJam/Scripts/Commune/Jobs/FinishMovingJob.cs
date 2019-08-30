using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class FinishMovingJob : Job
{
	public override bool IsAvailable( NPC_Commune npc )
	{
		return true;
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		Duration = NPC.MoveTime;
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Finish()
	{
		base.Finish();

		NPC.FindJob();
	}
}
