using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBush : MonoBehaviour
{
	public const float BushGrowthTime = 5;
	public const float BerryGrowthTime = 3;

	public enum Stage
	{
		Tilled,
		Planted,
		Grown,
		Harvestable
	}

	public GameObject Bush;
	public Berry[] Berries;

	[HideInInspector]
	public Stage CurrentStage;
	[HideInInspector]
	public NPC_Commune AssignedNPC;

	protected float StageTime = 0;

    void Start()
	{
		transform.GetChild( 0 ).localScale *= BuildableArea.Instance.CellSize;
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
				progress = Mathf.Max( 0, ( Time.time - StageTime ) / ( BerryGrowthTime / 2 ) - 1 );
				foreach ( var berry in Berries )
				{
					berry.transform.localScale = Vector3.one * progress;
				}
				if ( Time.time - StageTime >= BerryGrowthTime )
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

		// temp testing
		Bush.transform.localScale = Vector3.one;
	}

	public void StartBerries()
	{
		CurrentStage = Stage.Grown;
		StageTime = Time.time;

		// TODO Define positions for berries to appear?
	}

	public int Harvest()
	{
		StartBerries();
		// TODO different random? at least add to better variable control at top
		return Random.Range( 3, 6 );
	}
}
