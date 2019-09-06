using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for shared aspects such as appearance
public class NPC : MonoBehaviour
{
	PersonInfo Data;

    public bool IsWalking = false;

    public GameObject IdleSprite;
    public GameObject WalkingSprite;

	// Found using tags
	private List<GameObject> Hat;
	private List<GameObject> Sunglasses;
	private List<GameObject> Shirt;
	private List<GameObject> Pin;

	public virtual void Start()
    {
		Hat = transform.FindObjectsWithTag( "Hat" );
		Sunglasses = transform.FindObjectsWithTag( "Sunglasses" );
		Shirt = transform.FindObjectsWithTag( "Shirt" );
		Pin = transform.FindObjectsWithTag( "Pin" );

		// TODO TEMP REMOVE
		GenerateAppearanceFromData( PersonInfo.GenerateRandom( "DEBUG_FRIEND" ) );
	}

    public virtual void Update()
    {
        UpdateAnimations();
    }

	public void GenerateAppearanceFromData(PersonInfo newData)
	{
        Data = newData;

		foreach ( var obj in Hat )
		{
			obj.SetActive( Data.HasHat );

			var hatRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( hatRenderer != null )
			{
				hatRenderer.material.color = Data.HatColour;
			}

			obj.transform.localScale *= Data.HatScale;
		}
		foreach ( var obj in Sunglasses )
		{
			obj.SetActive( Data.HasSunglasses );

			var sunglassesRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( sunglassesRenderer != null )
			{
				sunglassesRenderer.material.color = Data.SunglassesColour;
			}

			obj.transform.localScale *= Data.SunglassesScale;
		}
		foreach ( var obj in Shirt )
		{
			obj.SetActive( Data.HasShirt );

			var shirtRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( shirtRenderer != null )
			{
				shirtRenderer.material.color = Data.ShirtColour;
			}
		}
		foreach ( var obj in Pin )
		{
			obj.SetActive( Data.HasPin );

			var pinRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( pinRenderer != null )
			{
				pinRenderer.material.color = Data.PinColour;
			}

			obj.transform.localScale *= Data.PinScale;
		}
	}

    public void UpdateAnimations()
    {
		// TODO turn in to state system to better support transitions and new animations
		WalkingSprite.SetActive( IsWalking );
		IdleSprite.SetActive( !IsWalking );
	}

    public PersonInfo GetPersonInfo()
    {
        return Data;
    }
}
