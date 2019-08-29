using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class NPC_Commune : NPC
{
	public enum JobType
	{
		Relax,
		Planting,
		Gathering,
		Cooking, // Includes bottling?
		Selling,
		Count
	}

	// Jobs
	public const int RelaxTrigger = 5;

	// Pathing
	public float MoveTime = 1;
	[HideInInspector]
	public Point Current;

	// Jobs
	protected int RelaxPoints = 0;
	protected JobType CurrentJob = JobType.Relax;
	protected Job[] JobClasses = new Job[] {
		new RelaxJob(),
		new PlantingJob(),
		new Job(),
		new Job(),
		new Job(),
		new Job(),
	};

	// Pathing
	protected Point Target;
	protected List<Point> Path = new List<Point>();
	protected float CurrentMoveTime = 0;

    void Start()
    {
		Current = new Point( Random.Range( 0, BuildableArea.Instance.GridSquares ), Random.Range( 0, BuildableArea.Instance.GridSquares ) );
		Target = Current;

		JobClasses[(int) CurrentJob].Start( this );
	}

    void Update()
    {
        // Debug Test pathfinding - If no target then choose random valid within grid
		if ( Path.Count <= 0 || Target == Current )
		{
			SetTargetCell( new Point( Random.Range( 0, BuildableArea.Instance.GridSquares ), Random.Range( 0, BuildableArea.Instance.GridSquares ) ) );
			CurrentMoveTime = Time.time;
		}

		UpdateMove();
		JobClasses[(int) CurrentJob].Update();
	}

	protected void UpdateMove()
	{
		if ( Path.Count > 0 )
		{
			// Lerp to each cell on the journey
			float progress = ( Time.time - CurrentMoveTime ) / MoveTime;
			Vector3 start = BuildableArea.GetPositionFromCell( Current.x, Current.y );
			Vector3 finish = BuildableArea.GetPositionFromCell( Path[0].x, Path[0].y );
			transform.position = Vector3.Lerp( start, finish, progress );
			transform.LookAt( finish );
			transform.localEulerAngles = new Vector3( 0, transform.localEulerAngles.y, 0 );

			// Set new current once reached grid cell
			if ( Time.time - CurrentMoveTime >= MoveTime )
			{
				Current = Path[0];
				Path.RemoveAt( 0 );
				CurrentMoveTime = Time.time;
			}
		}
	}

	protected void FindJob()
	{
		if ( RelaxPoints >= RelaxTrigger )
		{
			CurrentJob = JobType.Relax;
		}
		else
		{
			// Try to find a job open at each priority starting with highest, otherwise go back to relaxing
			for ( int job = (int) JobType.Count - 1; job >= 0; job-- )
			{
				if ( JobClasses[job].IsAvailable( this ) )
				{
					Debug.Log( "Start job: " + (JobType) job );
					CurrentJob = (JobType) job;
					JobClasses[(int) CurrentJob].Start( this );

					break;
				}
			}
		}
	}

	public void OnJobFinished()
	{
		Target = Current;
		FindJob();
	}

	public void SetTargetCell( Point cell )
	{
		Target = cell;
		Path = Pathfinding.FindPath( BuildableArea.Instance.Grid, Current, Target );
	}
}
