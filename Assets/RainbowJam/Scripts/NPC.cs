using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for shared aspects such as appearance
public class NPC : MonoBehaviour
{
    public bool IsWalking = false;
    bool isInitialised = false;
	public bool FlaggingGay = false;

    public GameObject IdleSprite;
    public GameObject WalkingSprite;

	protected PersonInfo Data;
    protected PersonalStory storyData;

	// Found using tags
	protected List<GameObject> Hat;
	protected List<GameObject> Shirt;
	protected List<GameObject> Hair1;
	protected List<GameObject> Hair2;
	protected List<GameObject> Hair3;
	protected List<GameObject> Flag_Pin;
	protected List<GameObject> Flag_Hanky;
	protected List<GameObject> Flag_Neckerchief;
	protected List<GameObject> Flag_Patch;
	protected List<GameObject> Flag_Backpatch;
	protected List<GameObject> Hoodie;
	protected List<GameObject> Vest;


    public virtual void Init(bool flagged = false)
    {
        Hat = transform.FindObjectsWithTag("Hat");
        Shirt = transform.FindObjectsWithTag("Shirt");
        Hair1 = transform.FindObjectsWithTag("Hair1");
        Hair2 = transform.FindObjectsWithTag("Hair2");
        Hair3 = transform.FindObjectsWithTag("Hair3");
        Flag_Pin = transform.FindObjectsWithTag("Flag_Pin");
        Flag_Neckerchief = transform.FindObjectsWithTag("Flag_Neckerchief");
        Flag_Patch = transform.FindObjectsWithTag("Flag_Patch");
        Flag_Backpatch = transform.FindObjectsWithTag("Flag_Backpatch");
        Flag_Hanky = transform.FindObjectsWithTag("Flag_Hanky");
        Hoodie = transform.FindObjectsWithTag("Hoodie");
        Vest = transform.FindObjectsWithTag("Vest");

        // TODO TEMP REMOVE
        GenerateAppearanceFromData(PersonInfo.GenerateRandom(JsonData.GetRandomName(), flagged));

        //Personal Story
        storyData = PersonalStory.GenerateRandom();

		FlaggingGay = flagged;
		isInitialised = true;
    }


	public virtual void Start()
    {
        if (!isInitialised)
            Init();
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
		foreach ( var obj in Shirt )
		{
			obj.SetActive( Data.HasShirt );

			var shirtRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( shirtRenderer != null )
			{
				shirtRenderer.material.color = Data.ShirtColour;
			}
		}
		foreach ( var obj in Hair1 )
		{
			obj.SetActive( Data.HasHair1 );

			var hair1Renderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( hair1Renderer != null )
			{
				hair1Renderer.material.color = Data.Hair1Color;
			}
		}
		foreach ( var obj in Hair2 )
		{
			obj.SetActive( Data.HasHair2 );

			var hair2Renderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( hair2Renderer != null )
			{
				hair2Renderer.material.color = Data.Hair2Color;
			}
		}
		foreach ( var obj in Hair3 )
		{
			obj.SetActive( Data.HasHair3 );

			var hair3Renderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( hair3Renderer != null )
			{
				hair3Renderer.material.color = Data.Hair3Color;
			}
		}
		foreach ( var obj in Hoodie )
		{
			obj.SetActive( Data.HasHoodie );

			var hoodieRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( hoodieRenderer != null )
			{
				hoodieRenderer.material.color = Data.HoodieColor;
			}
		}
		foreach ( var obj in Vest )
		{
			obj.SetActive( Data.HasVest );

			var vestRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( vestRenderer != null )
			{
				vestRenderer.material.color = Data.VestColor;
			}
		}
		foreach ( var obj in Flag_Pin )
		{
			obj.SetActive( Data.HasFlagPin );
		}
		foreach ( var obj in Flag_Backpatch )
		{
			obj.SetActive( Data.HasFlagBackpatch );
		}
		foreach ( var obj in Flag_Hanky )
		{
			obj.SetActive( Data.HasFlagHanky );
		}
		foreach ( var obj in Flag_Neckerchief )
		{
			obj.SetActive( Data.HasFlagNeckerchief );
		}
		foreach ( var obj in Flag_Patch )
		{
			obj.SetActive( Data.HasFlagPatch );
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

    public PersonalStory GetPersonalStory()
    {
        return storyData;
    }

	// Called only from city but there's no NPC_City and time is low
	public void Invite()
	{
		// TODO

		// Stop walking
		IsWalking = false;
		GetComponent<CityWander>().enabled = false;

		// Open dialogue
		SceneController.Instance.SummonDialogueBubble( JsonData.GetDialogueFromStoryID( storyData.storyID, PersonalStory.PersonalGoals.PART_1 ) );

		// Add friend to the commune with CityBridgingScript
		SceneController.Instance.CityBridge.AddFriend( Data );

		// Delete from this scene when dialogue done

		// TODO TEMP REMOVE
		SceneController.Instance.SwitchToCommune();
	}
}
