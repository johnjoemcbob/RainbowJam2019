using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Matthew 27/08/19
// There is a built in BillboardRenderer in Unity but I think that just faces the camera, this faces only on yaw axis
// Also using flat cube mesh renderers for characters to avoid z fighting with sprites etc
[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    void Update()
    {
		transform.LookAt( Camera.main.transform );
		transform.localEulerAngles = new Vector3( 0, transform.localEulerAngles.y, 0 );
    }
}
