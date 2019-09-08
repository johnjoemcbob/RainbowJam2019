using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_City : MonoBehaviour
{
	public const float InteractRange = 1;

	public float speed;

	[Header( "References" )]
	public GameObject IdleSprite;
	public GameObject WalkingSprite;

	protected Vector3 BaseScale;
	protected Vector3 InitialPos;

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
	protected List<GameObject> PuffyJacket;

	PersonInfo Data;

	private void Awake()
	{
		InitialPos = transform.position;

		Hat = transform.FindObjectsWithTag( "Hat" );
		Shirt = transform.FindObjectsWithTag( "Shirt" );
		Hair1 = transform.FindObjectsWithTag( "Hair1" );
		Hair2 = transform.FindObjectsWithTag( "Hair2" );
		Hair3 = transform.FindObjectsWithTag( "Hair3" );
		Flag_Pin = transform.FindObjectsWithTag( "Flag_Pin" );
		Flag_Neckerchief = transform.FindObjectsWithTag( "Flag_Neckerchief" );
		Flag_Patch = transform.FindObjectsWithTag( "Flag_Patch" );
		Flag_Backpatch = transform.FindObjectsWithTag( "Flag_Backpatch" );
		Flag_Hanky = transform.FindObjectsWithTag( "Flag_Hanky" );
		Hoodie = transform.FindObjectsWithTag( "Hoodie" );
		Vest = transform.FindObjectsWithTag( "Vest" );
		PuffyJacket = transform.FindObjectsWithTag( "PuffyJacket" );
	}

	private void OnEnable()
	{
		transform.position = InitialPos;
	}

	void Start()
    {
		BaseScale = WalkingSprite.transform.localScale;

		GenerateAppearanceFromData(PersonInfo.GenerateRandom("You", true));
	}

	public void GenerateAppearanceFromData(PersonInfo newData)
	{
        Data = newData;

		Data.HatColour = Color.white;
		Data.Hair1Color = Color.white;
		Data.Hair2Color = Color.white;
		Data.Hair3Color = Color.white;
		Data.VestColor = Color.white;
		Data.PuffyJacketColor = Color.white;
		Data.HoodieColor = Color.white;

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
		foreach ( var obj in PuffyJacket )
		{
			obj.SetActive( Data.HasPuffyJacket );

			var puffyJacketRenderer = obj.GetComponentInChildren<SpriteRenderer>();
			if ( puffyJacketRenderer != null )
			{
				puffyJacketRenderer.material.color = Data.PuffyJacketColor;
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

    void Update()
    {
		UpdateMove();
		UpdateInteract();
	}

	protected void UpdateMove()
	{
		float horizontal = Input.GetAxis("Horizontal") * speed * 0.5f;
		float vertical = Input.GetAxis("Vertical") * speed;

		transform.Translate( -horizontal, 0, -vertical );

		// Animation for player
		bool moving = ( horizontal + vertical ) != 0;
		IdleSprite.SetActive( !moving );
		WalkingSprite.SetActive( moving );
		WalkingSprite.transform.localScale = new Vector3( BaseScale.x * ( horizontal >= 0 ? 1 : -1 ), BaseScale.y, BaseScale.z );
	}

	protected void UpdateInteract()
	{
		// TODO could be button so rebindable by player
		// TODO maybe should be player mouse clicking so fine input possible, rather than search around vicinity

		// Each frame check distance to all friends to find any in range
		// (Currently only for the first found - could be for closest in case but this will do for now)
		GameObject interactable = null;
		foreach ( var friend in CrowdManager.Instance.Friends )
		{
			float distance = Vector3.Distance( transform.position, friend.transform.position );
			if ( distance <= InteractRange )
			{
				interactable = friend;
				break;
			}
		}

		// If in range show user input above their head
		foreach ( var friend in CrowdManager.Instance.Friends )
		{
            if (!friend.GetComponent<NPC>().hasBeenInvited)
            {
                friend.GetComponentInChildren<Canvas>(true).gameObject.SetActive(friend == interactable);
            }
		}

		// If clicked then activate their story dialogue
		if ( interactable != null && Input.GetKeyDown( KeyCode.E ) )
		{
            if (!interactable.GetComponent<NPC>().hasBeenInvited)
            {

                if (!IsCommuneFull())
                {
                    interactable.GetComponent<NPC>().Invite();
                    interactable.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
                    CollectFriend();
                }
                else
                {
                    //No space...
                    SceneController.Instance.SummonDialogueBubble("It looks like you don't have any space in your commune...you'll have to try again later :(");
                }
            }
		}
	}

    void CollectFriend()
    {
        GameObject collectionObj = GameObject.Find("CollectionScript");
        if (collectionObj != null)
        {
            CityCollectionHandler collectHandler = collectionObj.GetComponent<CityCollectionHandler>();

            if (collectHandler != null)
            {
                collectHandler.CollectNewFriend();
            }
        } 
    }

    bool IsCommuneFull()
    {
        bool full = false;

        GameObject collectionObj = GameObject.Find("CollectionScript");
        if (collectionObj != null)
        {
            CityCollectionHandler collectHandler = collectionObj.GetComponent<CityCollectionHandler>();

            if (collectHandler != null)
            {
                return collectHandler.IsCommuneFull();
            }
        }

        return full;
    }
}
