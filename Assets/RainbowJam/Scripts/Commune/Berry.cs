using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : Copyable
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

	public void InitWithType( BerryType type )
	{
		CurrentType = type;
		if ( BerryTypeToColourMap == null )
		{
			// Init berry type-colour mapping on first berry spawned.
			List<Color> candidateColours = new List<Color> {
				SidneyPalette.black,
				SidneyPalette.brown,
				SidneyPalette.red, 
				SidneyPalette.orange,
				SidneyPalette.yellow,
				SidneyPalette.green,
				SidneyPalette.cyan,
				SidneyPalette.blue,
				SidneyPalette.purple
			};


			BerryTypeToColourMap = new Dictionary<BerryType, Color>();

			// Randomly assign colours from the candidate list to the breed list.
			for ( int i = 0; i < 9; i++ )
			{
				var berryColour = candidateColours[Random.Range(0, candidateColours.Count)];
				BerryTypeToColourMap.Add( (BerryType) i, berryColour );
				candidateColours.Remove( berryColour );
			}
		}

		// Then just set up the colour with the breed type.
		BerrySpriteRenderer.sprite = BerrySprites[(int) type];
		BerrySpriteRenderer.material.color = BerryTypeToColourMap[type];
	}

	public void UpdateColourWithGrowthFactor( float growthProgress )
	{
		growthProgress = Mathf.Clamp( growthProgress, 0.0f, 1.0f );
		BerrySpriteRenderer.material.color = Color.Lerp( Color.white, BerryTypeToColourMap[CurrentType], growthProgress );
	}

	public override void OnCopy()
	{
		base.OnCopy();

		UpdateColourWithGrowthFactor( 1 );
	}
}
