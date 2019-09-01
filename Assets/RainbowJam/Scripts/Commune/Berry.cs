using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : MonoBehaviour
{

	public SpriteRenderer BerrySpriteRenderer;

	public Sprite[] BerrySprites;

	public enum BerryType
	{
		STRAWBERRY = 0,
		BLUEBERRY,
		RASPBERRY,
		CLOUDBERRY,
		ROWANBERRY,
		GOOSEBERRY,
		CRANBERRY,
		GOJIBERRY,
		GRAPES
	}

	public static Dictionary<BerryType, Color> BerryTypeToColourMap;

	public BerryType CurrentType;

	public void InitWithType(BerryType type)
	{
		CurrentType = type;
		if(BerryTypeToColourMap == null)
		{
			// Init berry type-colour mapping on first berry spawned.
			List<Color> candidateColours = new List<Color> {
				new Color(1.0f, 0.10f, 0.0f, 1.0f), // rich red
				new Color(1.0f, 0.5f, 0.0f, 1.0f), // orange
				new Color(1.0f, 1.0f, 0.0f, 1.0f), // yellow
				new Color(0.25f, 1.0f, 0.05f, 1.0f), // lime green
				new Color(0.1f, 0.9f, 1.0f, 1.0f), // cyan
				new Color(0.10f, 0.0f, 1.0f), // dark blue
				new Color(0.6f, 0.0f, 1.0f, 1.0f), // purple
				new Color(0.05f, 0.05f, 0.05f, 1.0f), // black (slightly lighter than berry outlines)
				new Color(0.65f, 0.15f, 0.0f, 1.0f) // brown
			};

			BerryTypeToColourMap = new Dictionary<BerryType, Color>();

			// Randomly assign colours from the candidate list to the breed list.
			for(int i = 0; i < 9; i++)
			{
				var berryColour = candidateColours[Random.Range(0, candidateColours.Count)];
				BerryTypeToColourMap.Add((BerryType)i, berryColour);
				candidateColours.Remove(berryColour);
			}
		}

		// Then just set up the colour with the breed type.
		BerrySpriteRenderer.sprite = BerrySprites[(int)type];
		BerrySpriteRenderer.material.color = BerryTypeToColourMap[type];
	}

	public void UpdateColourWithGrowthFactor(float growthProgress)
	{
		growthProgress = Mathf.Clamp(growthProgress, 0.0f, 1.0f);
		BerrySpriteRenderer.material.color = Color.Lerp(Color.white, BerryTypeToColourMap[CurrentType], growthProgress );
	}

	

}
