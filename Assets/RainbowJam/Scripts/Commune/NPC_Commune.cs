﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NesScripts.Controls.PathFind;

public class NPC_Commune : NPC
{
	public enum JobType
	{
		Relax,
		Planting,
		Gathering,
		Selling,
		Cooking, // Includes bottling
		Story, // Find player and tell them part of their story
		FinishMoving, // Wait for NPC to stop moving before new job is assigned
		Count
	}

	// Jobs
	public const int RelaxTrigger = 5;
	// TODO could be better, more modular carrying code
	[HideInInspector]
	public int Berries = 0;

	[HideInInspector]
	public Berry.BerryType HeldBerryType;

	[HideInInspector]
	public int Jams = 0;

	// Pathing
	public float MoveTime = 1;
	[HideInInspector]
	public Point CurrentPos;

	// Jobs
	protected int RelaxPoints = 0;
	protected JobType CurrentJob = JobType.Relax;
	protected Job[] JobClasses = new Job[] {
		new RelaxJob(),
		new PlantingJob(),
		new GatheringJob(),
		new SellingJob(),
		new CookingJob(),
		new StoryJob(),
		new FinishMovingJob(),
		new Job(), // Count
	};
	protected GameObject CurrentlyHeld;

	// Pathing
	protected Point TargetPos;
	protected List<Point> Path = new List<Point>();
	protected float CurrentMoveTime = 0;

    public override void Start()
    {
		base.Start();

		CurrentPos = new Point( Random.Range( 0, BuildableArea.Instance.GridSquares ), Random.Range( 0, BuildableArea.Instance.GridSquares ) );
		TargetPos = CurrentPos;

		JobClasses[(int) CurrentJob].Start( this );


        // Nametag UI
        GameObject debugObj = transform.Find("Canvas/NameTag").gameObject;
        debugObj.SetActive(true);
        var text = debugObj.GetComponentInChildren<Text>();
        text.text = GetPersonInfo().Name;

        var bg = debugObj.GetComponentInChildren<Image>();
        bg.rectTransform.sizeDelta = new Vector2(text.preferredWidth + 50.0f, bg.rectTransform.sizeDelta.y);
    }

    public override void Update()
    {
		base.Update();

		UpdateDebug();
		UpdateMove();
		JobClasses[(int) CurrentJob].Update();
	}

	protected void UpdateDebug()
	{
        // Debug UI
        GameObject debugObj = transform.Find("Canvas/DebugDisplay").gameObject;
        debugObj.SetActive(Player_Commune.DebugDisplay);

        var text = debugObj.GetComponentInChildren<Text>(true);
        text.text = @"
		" + CurrentJob + " - Duration: " + JobClasses[(int) CurrentJob].GetTimeRemaining().ToString( "0.00" ) + "/" + JobClasses[(int) CurrentJob].Duration.ToString( "0.00" ) + @"
		" + CurrentPos.x + ", " + CurrentPos.y + " to " + TargetPos.x + ", " + TargetPos.y + @"
		Berries: " + Berries + @"
		RelaxPoints: " + RelaxPoints + @"
		LeftHanded: " + Data.LeftHanded + @"
		";
	}

	protected void UpdateMove()
	{
		IsWalking = false;
		if ( Path.Count > 0 )
		{
			// Lerp to each cell on the journey
			float progress = ( Time.time - CurrentMoveTime ) / MoveTime;
			Vector3 start = BuildableArea.GetPositionFromCell( CurrentPos.x, CurrentPos.y );
			Vector3 finish = BuildableArea.GetPositionFromCell( Path[0].x, Path[0].y );
			transform.position = Vector3.Lerp( start, finish, progress );
			if ( !GetComponent<Billboard>().enabled )
			{
				transform.LookAt( finish );
				transform.localEulerAngles = new Vector3( 0, transform.localEulerAngles.y, 0 );
			}

			UpdateFacing( start, finish );
			UpdateParentItemToCurrentHand();

			// Set new current once reached grid cell
			if ( Time.time - CurrentMoveTime >= MoveTime )
			{
				CurrentPos = Path[0];
				Path.RemoveAt( 0 );
				CurrentMoveTime = Time.time;

				// TODO Testing regen path each tile, to avoid other NPCs/player
				SetTargetCell( TargetPos );
			}
		}
		else if ( CurrentPos != TargetPos )
		{
			// Try to find new path if they still have a target unreached
			SetTargetCell( TargetPos );
		}
	}

	// Calculate facing direction (Melon)
	protected void UpdateFacing( Vector3 start, Vector3 finish )
	{
		Vector3 travelDir = (finish - start);
		travelDir.Normalize();

		float angBetween = Vector3.Dot(travelDir, transform.right);
		//Debug.Log(angBetween);
		if ( angBetween > 0 )
		{
			WalkingSprite.transform.localEulerAngles = new Vector3( 0, 0, 0 );
			// Update z offset of all children to be in front of animation
			foreach ( Transform child in WalkingSprite.transform )
			{
				child.localPosition = new Vector3( child.localPosition.x, child.localPosition.y, Mathf.Abs( child.localPosition.z ) );
			}
		}
		else
		{
			WalkingSprite.transform.localEulerAngles = new Vector3( 0, 180, 0 );
			// Update z offset of all children to be in front of animation
			foreach ( Transform child in WalkingSprite.transform )
			{
				child.localPosition = new Vector3( child.localPosition.x, child.localPosition.y, -Mathf.Abs( child.localPosition.z ) );
			}
		}
		IsWalking = true;
	}

	public void FindJob()
	{
		if ( RelaxPoints >= RelaxTrigger )
		{
			CurrentJob = JobType.Relax;
		}
		else
		{
			// Try to find a job open at each priority starting with highest, otherwise go back to relaxing
			// -2 to ignore FinishMoving
			for ( int job = (int) JobType.Count - 2; job >= 0; job-- )
			{
				if ( JobClasses[job].IsAvailable( this ) )
				{
					CurrentJob = (JobType) job;

					break;
				}
			}
		}

		Debug.Log( "Start job: " + CurrentJob );
		JobClasses[(int) CurrentJob].Start( this );

		// Currently relaxing, reset points
		if ( CurrentJob == JobType.Relax )
		{
			RelaxPoints = 0;
		}
	}

	public void OnJobFinished()
	{
		// Cancel any movement
		if ( Path.Count > 0 )
		{
			SetTargetCell( Path[0] );
		}
		else
		{
			SetTargetCell( CurrentPos );
		}

		// Flag to find new job once stopped moving
		CurrentJob = JobType.FinishMoving;
		JobClasses[(int) CurrentJob].Start( this );
	}

	public void SetTargetCell( Point cell )
	{
		TargetPos = cell;
		Path = Pathfinding.FindPath( BuildableArea.Instance.GridWithMovables, CurrentPos, TargetPos );
	}

	// Only generate path if it's actually new
	public void SetIfNotTargetCell( Point cell )
	{
		if ( TargetPos != cell )
		{
			SetTargetCell( cell );
		}
	}

	public void PickupItem( GameObject item )
	{
		// Make a copy of the item and store it on NPC
		CurrentlyHeld = Instantiate( item );
		UpdateParentItemToCurrentHand();
		var copy = CurrentlyHeld.GetComponent<Copyable>();
		if ( copy != null )
		{
			copy.OnCopy();
		}
	}

	public void DropItem()
	{
		if ( CurrentlyHeld != null )
		{
			Destroy( CurrentlyHeld );
			CurrentlyHeld = null;
		}
	}

	public void TalkToPlayer()
	{
		// Open dialogue and fill
		SceneController.Instance.SummonDialogueBubble( ParseStorySegment( JsonData.GetDialogueFromStoryID( Data.StoryData.storyID, Data.StoryData.GetCurrentStage() ) ) );
		Data.StoryData.TalkToPlayer();
	}

	protected void UpdateParentItemToCurrentHand()
	{
		if ( CurrentlyHeld == null ) return;

		Transform parent = IdleSprite.transform;
		if ( WalkingSprite.activeSelf )
		{
			parent = WalkingSprite.transform;
		}
		bool lefthanded = Data.LeftHanded;
		if ( WalkingSprite.transform.localEulerAngles.y == 180 )
		{
			lefthanded = !lefthanded;
		}
		string hand = lefthanded ? "Left" : "Right";
		CurrentlyHeld.transform.parent = parent.Find( "Held " + hand + " (Pivot)" );
		CurrentlyHeld.transform.localPosition = Vector3.zero;
		CurrentlyHeld.transform.localEulerAngles = Vector3.zero;
		CurrentlyHeld.transform.localScale = Vector3.one * 50;
	}

	public void TaskComplete()
	{
		RelaxPoints++;
	}
}
