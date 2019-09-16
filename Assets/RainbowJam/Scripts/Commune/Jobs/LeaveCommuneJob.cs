using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class LeaveCommuneJob : Job
{
	protected Point TargetPos;

	public override bool IsAvailable( NPC_Commune npc )
	{
		// TODO leave if story segment is 3 and wants talk to player is false
		return (
			( ( npc.GetPersonalStory().GetCurrentStage() == PersonalStory.PersonalGoals.PART_3 ) ||
			( npc.GetPersonalStory().GetCurrentStage() == PersonalStory.PersonalGoals.PART_4 ) ) &&
			!CommuneToControllerBridge.Instance.IsParty &&
			!npc.GetPersonalStory().GetWantsToTalk()
		);
	}

	public override void Start( NPC_Commune npc )
	{
		base.Start( npc );

		// With timeout duration also
		Duration = Random.Range( 5, 10 );
		TargetPos = BuildableArea.GetCellFromPosition( GameObject.FindObjectOfType<CityTravelZone>().transform.position );
		NPC.SetIfNotTargetCell( TargetPos );
	}

	public override void Update()
	{
		base.Update();

		if ( NPC.CurrentPos == TargetPos )
		{
			Finish();
		}
	}

	public override void Finish()
	{
		base.Finish();

		NPC.gameObject.SetActive( false );
	}
}
