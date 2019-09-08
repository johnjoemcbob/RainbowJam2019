using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBush : MonoBehaviour
{
	public const float BushGrowthTime = 5;
	public static Vector2 BerryGrowthTime = new Vector2( 10, 20 );

	public enum Stage
	{
		Tilled,
		Planted,
		Grown,
		Harvestable
	}

	public GameObject Bush;
	public Berry[] Berries;
	public AudioSource TillSoilSound;
	public AudioSource BerryPickSound;

	[HideInInspector]
	public Stage CurrentStage;
	[HideInInspector]
	public NPC_Commune AssignedNPC;

	protected float StageTime = 0;
	protected float GrowTime = 0;

	public Berry.BerryType GrowBerryType;

    void Start()
	{
		transform.GetChild( 0 ).localScale *= BuildableArea.Instance.CellSize;

		// Ensure data grid is filled properly (in case added in editor)
		var cell = BuildableArea.GetCellFromPosition( gameObject );
		BuildableArea.Instance.Grid.nodes[cell.x, cell.y].Type = NesScripts.Controls.PathFind.NodeContent.TilledSoil;

		GrowBerryType = (Berry.BerryType)Random.Range(0,9);

		TillSoilSound.pitch = Random.Range(0.85f, 1.05f);
		TillSoilSound.Play();
	}

    void Update()
    {
		float progress;
		switch ( CurrentStage )
		{
			case Stage.Tilled:
				break;
			case Stage.Planted:
				progress = ( Time.time - StageTime ) / BushGrowthTime;
				Bush.transform.localScale = Vector3.one * progress;
				if ( Time.time - StageTime >= BushGrowthTime )
				{
					StartBerries();
				}

				break;
			case Stage.Grown:
				progress = Mathf.Max( 0, ( Time.time - StageTime ) / ( GrowTime / 2 ) - 1 );
				foreach ( var berry in Berries )
				{
					berry.transform.localScale = new Vector3(0.5f, 1.0f, 1.0f) * progress;
					berry.UpdateColourWithGrowthFactor(progress);
				}
				if ( Time.time - StageTime >= GrowTime )
				{
					CurrentStage = Stage.Harvestable;
					StageTime = Time.time;
				}
				break;
			case Stage.Harvestable:
				break;
			default:
				break;
		}
	}

	public void Plant()
	{
		CurrentStage = Stage.Planted;
		Bush.SetActive( true );
		Bush.transform.localScale = Vector3.zero;
		StageTime = Time.time;
	}

	public void StartBerries()
	{
		CurrentStage = Stage.Grown;
		StageTime = Time.time;
		GrowTime = Random.Range( BerryGrowthTime.x, BerryGrowthTime.y );

		foreach(var berry in Berries)
		{
			berry.InitWithType(GrowBerryType);
		}

		// TODO Define positions for berries to appear?
	}

	public int Harvest()
	{
		BerryPickSound.pitch = Random.Range(0.85f, 1.25f);
		BerryPickSound.Play();
		StartBerries();
		// TODO different random? at least add to better variable control at top
		return Random.Range( 3, 6 );
	}
}
