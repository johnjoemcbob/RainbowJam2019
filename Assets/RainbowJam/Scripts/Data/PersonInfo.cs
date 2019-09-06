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
	public bool HasPin = false;
	public Color PinColour = Color.white;
	public float PinScale = 1.0f;

	public bool HasShirt = false;
	public Color ShirtColour = Color.white;

	public bool HasSunglasses = false;
	public Color SunglassesColour = Color.white;
	public float SunglassesScale = 1.0f;

	public bool HasHat = false;
	public Color HatColour = Color.white;
	public float HatScale = 1.0f;

	public bool LeftHanded;

	/// <summary>
	/// Helper method to generate random people (minus name generation)
	/// </summary>
	public static PersonInfo GenerateRandom( string name )
	{
		var newPerson = new PersonInfo();

		newPerson.Name = name;

		newPerson.HasShirt = ( Random.Range( 0.0f, 1.0f ) > 0.5f );
		newPerson.HasHat = ( Random.Range( 0.0f, 1.0f ) > 0.5f );
		newPerson.HasPin = ( Random.Range( 0.0f, 1.0f ) > 0.5f );
		newPerson.HasSunglasses = ( Random.Range( 0.0f, 1.0f ) > 0.5f );

		newPerson.ShirtColour = Random.ColorHSV( 0.0f, 1.0f, 0.8f, 1.0f, 0.8f, 1.0f );
		newPerson.HatColour = Random.ColorHSV( 0.0f, 1.0f, 0.8f, 1.0f, 0.8f, 1.0f );
		newPerson.SunglassesColour = Random.ColorHSV( 0.0f, 1.0f, 0.8f, 1.0f, 0.8f, 1.0f );
		newPerson.PinColour = Random.ColorHSV( 0.0f, 1.0f, 0.8f, 1.0f, 0.8f, 1.0f );

		newPerson.HatScale = Random.Range( 0.95f, 1.10f );
		newPerson.PinScale = Random.Range( 0.85f, 1.45f );
		newPerson.SunglassesScale = Random.Range( 0.95f, 1.10f );

		newPerson.LeftHanded = Random.Range( 0, 2 ) == 0; // 50% chance

		return newPerson;
	}
}
