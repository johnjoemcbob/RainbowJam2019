using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
	public float Duration = 0;

	protected float StartTime = 0;
	protected NPC_Commune NPC;

	public virtual bool IsAvailable( NPC_Commune npc )
	{
		return false;
	}

	public virtual void Start( NPC_Commune npc )
	{
		NPC = npc;
		Duration = 0;
		StartTime = Time.time;
	}

	public virtual void Update()
	{
		if ( Duration != 0 && Time.time - StartTime >= Duration )
		{
			Finish();
		}
	}

	public virtual void Finish()
	{
		if ( NPC != null )
		{
			NPC.OnJobFinished();
		}
	}
}
