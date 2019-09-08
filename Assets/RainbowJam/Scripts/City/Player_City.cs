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

    void Start()
    {
		BaseScale = WalkingSprite.transform.localScale;
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
			friend.GetComponentInChildren<Canvas>( true ).gameObject.SetActive( friend == interactable );
		}

		// If clicked then activate their story dialogue
		if ( Input.GetKeyDown( KeyCode.E ) )
		{
			interactable.GetComponent<NPC>().Invite();
		}
	}
}
