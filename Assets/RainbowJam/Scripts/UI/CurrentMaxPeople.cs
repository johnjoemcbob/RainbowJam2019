using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof( Text ) )]
public class CurrentMaxPeople : MonoBehaviour
{
	protected Text text;

	private void Awake()
	{
		text = GetComponentInChildren<Text>();
	}

	void Update()
    {
		text.text = CommuneToControllerBridge.GetCurrentCommuneNPCs() + "/" + CommuneToControllerBridge.CurrentMaxPeople;
    }
}
