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

	private void Awake()
	{
		Instance = this;

		Grid = new NesScripts.Controls.PathFind.Grid( GridSquares, GridSquares );

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
		// Does the ray intersect any objects excluding the player layer
		RaycastHit hit;
		int layerMask = ~( 1 << 8 );
		if ( Physics.Raycast( Camera.main.transform.position, Camera.main.transform.TransformDirection( Vector3.forward ), out hit, Mathf.Infinity, layerMask ) )
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
		Test2.GetComponent<Renderer>().material.color = Grid.nodes[(int) gridcell.x, (int) gridcell.z].Type == NodeContent.Impasse ? Color.red : Color.white;

		// User input, TODO maybe move to update so it's clearer
		if ( Input.GetMouseButtonDown( 0 ) )
		{
			PlaceObject( gridcell.x, gridcell.z, NodeContent.TilledSoil );
		}
		if ( Input.GetMouseButtonDown( 1 ) )
		{
			FindObjectOfType<NPC_Commune>().SetTargetCell( new Point( (int) gridcell.x, (int) gridcell.z ) );
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
