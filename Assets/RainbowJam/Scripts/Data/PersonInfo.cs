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

    // Things like personality, or if they are friend-or-foe, is probably something that should be handled in the relevant scenes - this is just a way to carry essential spawn info around.  
}
