using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableArea : MonoBehaviour
{
	// Assumes square grid of squares for ease of setup
	public float GridSize = 1;
	public int GridSquares = 32;

	public GameObject Test;
	public GameObject Test2;

	void Start()
    {
        
    }

	void Update()
	{
		// Does the ray intersect any objects excluding the player layer
		RaycastHit hit;
		int layerMask = ~( 1 << 8 );
		if ( Physics.Raycast( Camera.main.transform.position, Camera.main.transform.TransformDirection( Vector3.forward ), out hit, Mathf.Infinity, layerMask ) )
		{
			var buildarea = hit.collider.gameObject.GetComponentInParent<BuildableArea>();
			if ( buildarea != null )
			{
				buildarea.RayHit( hit );
			}
		}
	}

	private void RayHit( RaycastHit hit )
	{
		Vector3 gridpoint = new Vector3( hit.point.x - ( hit.point.x % GridSize ), hit.point.y, hit.point.z - ( hit.point.z % GridSize ) );
		gridpoint += new Vector3( 1, 0, 1 ) * GridSize / 2;
		Test.transform.position = hit.point;
		Test2.transform.position = gridpoint;
	}
}
