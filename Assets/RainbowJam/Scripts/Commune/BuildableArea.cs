using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class BuildableArea : MonoBehaviour
{
	public static BuildableArea Instance;

	[Header( "Variables" )]
	// Assumes square grid of squares for ease of setup
	public float CellSize = 1;
	public int GridSquares = 32;

	public Rect[] DefaultImpasseAreas;

	[Header( "References" )]
	public GameObject[] ContentPrefabs;
	public GameObject Test;
	public GameObject Test2;

	[HideInInspector]
	public NesScripts.Controls.PathFind.Grid Grid;
	public NesScripts.Controls.PathFind.Grid GridWithMovables;

	private void Awake()
	{
		Instance = this;

		Grid = new NesScripts.Controls.PathFind.Grid( GridSquares, GridSquares );
		GridWithMovables = new NesScripts.Controls.PathFind.Grid( GridSquares, GridSquares );

		// Add default impasses (e.g. house etc)
		foreach ( var area in DefaultImpasseAreas )
		{
			for ( int x = (int) area.x; x < area.x + area.width; x++ )
			{
				for ( int y = (int) area.y; y < area.y + area.height; y++ )
				{
					Grid.nodes[x, y].Type = NodeContent.Impasse;
					Grid.nodes[x, y].walkable = false;
					Grid.nodes[x, y].price = 0;
				}
			}
		}
	}

	void Start()
    {
    }

	void Update()
	{
		// Each frame, copy base grid over and add npc/players as impasse
		for ( int x = 0; x < GridSquares; x++ )
		{
			for ( int z = 0; z < GridSquares; z++ )
			{
				GridWithMovables.nodes[x, z].Type = Grid.nodes[x, z].Type;
				GridWithMovables.nodes[x, z].walkable = Grid.nodes[x, z].walkable;
				GridWithMovables.nodes[x, z].price = Grid.nodes[x, z].price;
			}
		}
		// Player
		var cell = GetCellFromPosition( Player_Commune.Instance.gameObject );
		GridWithMovables.nodes[cell.x, cell.y].Type = NodeContent.Impasse;
		GridWithMovables.nodes[cell.x, cell.y].price = 100;
		// NPCs
		foreach ( var npc in FindObjectsOfType<NPC_Commune>() )
		{
			cell = GetCellFromPosition( npc.gameObject );
			GridWithMovables.nodes[cell.x, cell.y].Type = NodeContent.Impasse;
			GridWithMovables.nodes[cell.x, cell.y].price = 10;
		}

		// Raycast for player interaction
		RaycastHit hit;
		int layerMask = ~( 1 << 8 );
		if ( Physics.Raycast( Camera.main.transform.position, Camera.main.transform.TransformDirection( Vector3.forward ), out hit, Player_Commune.MaxRange, layerMask ) )
		{
			var buildarea = hit.collider.gameObject.GetComponentInParent<BuildableArea>();
			if ( buildarea != null )
			{
				buildarea.RayHit( hit );
			}
		}
	}

	private void RayHit( RaycastHit hit )
	{
		Vector3 gridpoint = new Vector3( hit.point.x - ( hit.point.x % CellSize ), hit.point.y, hit.point.z - ( hit.point.z % CellSize ) );
		Vector3 gridcell = gridpoint / CellSize;
		gridpoint += new Vector3( 1, 0, 1 ) * CellSize / 2;

		// Testing/debug
		//Debug.Log( gridcell );
		Test.transform.position = hit.point;
		Test2.transform.position = gridpoint;
		Test2.GetComponent<Renderer>().material.color = GridWithMovables.nodes[(int) gridcell.x, (int) gridcell.z].Type == NodeContent.Impasse ? Color.red : Color.white;

		// User input, TODO maybe move to update so it's clearer
		if ( DialogueBubble.QueuedDialogues.Count < 1) // don't do building inputs while a dialogue is still in view.
		{
			if ( Input.GetMouseButtonDown( 0 ) )
			{
				if ( Grid.nodes[(int) gridcell.x, (int) gridcell.z].Type == NodeContent.Empty )
				{
					PlaceObject( gridcell.x, gridcell.z, NodeContent.TilledSoil );
					Player_Commune.Instance.Swing( 1 );
				}
			}
			if ( Input.GetMouseButtonDown( 1 ) )
			{
				FindObjectOfType<NPC_Commune>().SetTargetCell( new Point( (int) gridcell.x, (int) gridcell.z ) );
			}
		}
	}

	public void PlaceObject( float x, float z, NodeContent type )
	{
		PlaceObject( (int) x, (int) z, type );
	}
	public void PlaceObject( int x, int z, NodeContent type )
	{
		// Look up prefab based on CellContent
		GameObject obj = Instantiate( ContentPrefabs[(int)type], transform );
		obj.transform.position = GetPositionFromCell( x, z );

		// Store info
		Grid.nodes[x, z].Type = type;
		Grid.nodes[x, z].Reference = obj;
		Grid.nodes[x, z].price = 10;// 2;
	}

	public static Vector3 GetPositionFromCell( int x, int z )
	{
		return new Vector3( x + 0.5f, 0, z + 0.5f ) * Instance.CellSize;
	}

	public static Point GetCellFromPosition( GameObject obj )
	{
		return GetCellFromPosition( obj.transform.position );
	}
	public static Point GetCellFromPosition( Vector3 pos )
	{
		Vector3 gridpoint = new Vector3( pos.x - ( pos.x % Instance.CellSize ), pos.y, pos.z - ( pos.z % Instance.CellSize ) );
		Vector3 gridcell = gridpoint / Instance.CellSize;
		return new Point( (int) gridcell.x, (int) gridcell.z );
	}
}
