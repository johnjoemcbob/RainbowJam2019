using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBush : MonoBehaviour
{
	public const float GrowthTime = 1;

	public enum Stage
	{
		Tilled,
		Planted,
		Grown
	}

	public GameObject Bush;

	[HideInInspector]
	public Stage CurrentStage;
	//[HideInInspector]
	public NPC_Commune AssignedNPC;

	protected float PlantTime = 0;

    void Start()
	{
		transform.GetChild( 0 ).localScale *= BuildableArea.Instance.CellSize;
	}

    void Update()
    {
		switch ( CurrentStage )
		{
			case Stage.Tilled:
				break;
			case Stage.Planted:
				float progress = ( Time.time - PlantTime ) / GrowthTime;
				Bush.transform.localScale = Vector3.one * progress;
				if ( Time.time - PlantTime >= GrowthTime )
				{
					CurrentStage = Stage.Grown;
				}

				break;
			case Stage.Grown:
				break;
			default:
				break;
		}
	}

	public void Plant()
	{
		CurrentStage = Stage.Planted;
		Bush.SetActive( true );
		PlantTime = Time.time;

		// temp testing
		Bush.transform.localScale = Vector3.one;
	}
}
