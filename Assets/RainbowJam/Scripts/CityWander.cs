using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityWander : MonoBehaviour
{
	public static float Speed = 0.05f;

	protected Vector3 StartPos;
	protected Vector3 Target;
	protected Vector3 Direction;

	protected float NextChange = 0;
	protected float BetweenChange = 1;

	private void Start()
	{
		StartPos = transform.localPosition;
		RandomiseDirection();
	}

	void Update()
    {
		if ( Time.time >= NextChange )
		{
			RandomiseDirection();
		}
		transform.localPosition = Vector3.Lerp( transform.localPosition, Target, Time.deltaTime * Speed );
    }

	protected void RandomiseDirection()
	{
		Direction = new Vector3( Random.Range( -1, 1 ), 0, Random.Range( -1, 1 ) );
		BetweenChange = Random.Range( 2, 10 );
		NextChange = Time.deltaTime + BetweenChange;
		Target = StartPos + Direction * 5;// * Mathf.Sin( Time.time );
	}
}
