using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_City : MonoBehaviour
{
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
        float horizontal = Input.GetAxis("Horizontal") * speed * 0.5f;
        float vertical = Input.GetAxis("Vertical") * speed;

        transform.Translate(-horizontal, 0, -vertical);

		// Animation for player
		bool moving = ( horizontal + vertical ) != 0;
		IdleSprite.SetActive( !moving );
		WalkingSprite.SetActive( moving );
		WalkingSprite.transform.localScale = new Vector3( BaseScale.x * ( horizontal >= 0 ? 1 : -1 ), BaseScale.y, BaseScale.z );
	}
}
