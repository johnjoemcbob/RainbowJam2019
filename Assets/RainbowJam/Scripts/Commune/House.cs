using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public static House Instance;

	public GameObject[] Stories;
	public GameObject Roof;

	protected int story = -1;

	private void Awake()
	{
		Instance = this;

		// Setup, disable all and add ground floor
		foreach ( var story in Stories )
		{
			story.SetActive( false );
		}
		AddStory();
	}

	public void AddStory()
	{
		story++;
		Stories[story].SetActive( true );
		Roof.transform.position = Stories[story].transform.position + new Vector3( 0, 2.0625f, 0 );

		if(story > 0)
		{
			CommuneToControllerBridge bridgeScript = GetComponentInParent<CommuneToControllerBridge>();
			if(bridgeScript != null)
			{
				bridgeScript.SummonDialogueBubble("Everyone's hard work has made the house bigger!");
			}
			CommuneToControllerBridge.CurrentMaxPeople = CommuneToControllerBridge.Instance.MaxPeopleBuildingUpgradeStages[story];
		}
	}
}
