using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple data structure representing one of the lil' person's defining characteristics.
/// This will let us use a container (like a List<PersonInfo>) to carry the information of people fetched from the city scene to the commune scene
/// without much trouble; we can then add a function the in-game object, something like "InstantiateFromInfo(PersonInfo newInfo)" that does all the correct
/// sprite-layering and so on for the character, when we spawn them.
/// </summary>
public class PersonInfo
{
	public string Name = "Unnamed Friend"; // Might not need names, but still handy as an ID field.

	// A bunch of fields for their accessories info?
	public bool HasFlagPin = false;
	public bool HasFlagBackpatch = false;
	public bool HasFlagHanky = false;
	public bool HasFlagPatch = false;
	public bool HasFlagNeckerchief = false;

	public bool HasHair1 = false;
	public bool HasHair2 = false;
	public bool HasHair3 = false;
	public Color Hair1Color = Color.white;
	public Color Hair2Color = Color.white;
	public Color Hair3Color = Color.white;

	public bool HasShirt = false;
	public Color ShirtColour = Color.white;

	public bool HasHat = false;
	public Color HatColour = Color.white;
	public float HatScale = 1.0f;

	public bool LeftHanded;

	/// <summary>
	/// Helper method to generate random people (minus name generation)
	/// </summary>
	public static PersonInfo GenerateRandom( string name, bool flagged = false )
	{
		var newPerson = new PersonInfo();

		newPerson.Name = name;

		newPerson.HasShirt = ( Random.Range( 0.0f, 1.0f ) > 0.5f );
		newPerson.HasHat = ( Random.Range( 0.0f, 1.0f ) > 0.5f );
		SetRandomHair(ref newPerson);


		newPerson.ShirtColour = SidneyPalette.ChooseRandom();
		newPerson.HatColour = SidneyPalette.ChooseRandom();
		newPerson.Hair1Color = SidneyPalette.ChooseRandom();
		newPerson.Hair2Color = SidneyPalette.ChooseRandom();
		newPerson.Hair3Color = SidneyPalette.ChooseRandom();

		if(flagged)
		{
			ChooseFlaggedItems(ref newPerson);
		}



		newPerson.HatScale = Random.Range( 0.95f, 1.10f );

		newPerson.LeftHanded = Random.Range( 0, 2 ) == 0; // 50% chance

		return newPerson;
	}

	public static void ChooseFlaggedItems(ref PersonInfo info)
	{
		// i know there are better ways to ensure a minimum random outcome
		// but this is fine >:)

		while(!info.HasFlagBackpatch &&
			  !info.HasFlagHanky&&
			  !info.HasFlagNeckerchief &&
			  !info.HasFlagPatch&&
			  !info.HasFlagPin)
			  {
				  
				  info.HasFlagBackpatch = Random.Range(0, 2) == 0;
				  info.HasFlagHanky = Random.Range(0, 2) == 0;
				  info.HasFlagNeckerchief = Random.Range(0, 2) == 0;
				  info.HasFlagPatch = Random.Range(0, 2) == 0;
				  info.HasFlagPin = Random.Range(0, 2) == 0;
				  
			  }
	}

	public static void SetRandomHair(ref PersonInfo info)
	{
		int hairType = Random.Range(0, 4);
		switch(hairType)
		{
			case 0: // bald
				info.HasHair1 = false;
				info.HasHair2 = false;
				info.HasHair3 = false;
				break;
			case 1: // hair1
				info.HasHair1 = true;
				info.HasHair2 = false;
				info.HasHair3 = false;
				break;
			case 2:
				info.HasHair1 = false;
				info.HasHair2 = true;
				info.HasHair3 = false;
				break;
			case 3:
				info.HasHair1 = false;
				info.HasHair2 = false;
				info.HasHair3 = true;
				break;

		}
	}
}
