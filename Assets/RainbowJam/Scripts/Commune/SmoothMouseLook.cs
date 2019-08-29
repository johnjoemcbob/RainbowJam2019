using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu( "Camera-Control/Smooth Mouse Look" )]
public class SmoothMouseLook : MonoBehaviour
{
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;

	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;

	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;

	public float frameCounter = 20;

	Quaternion originalRotation;

	void Update()
	{
		rotationY += Input.GetAxis( "Mouse Y" ) * sensitivityY;
		rotationX += Input.GetAxis( "Mouse X" ) * sensitivityX;

		rotationX = ClampAngle( rotationX, minimumX, maximumX );
		rotationY = ClampAngle( rotationY, minimumY, maximumY );

		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);

		transform.parent.localRotation = originalRotation * xQuaternion;
		transform.localRotation = originalRotation * yQuaternion;
	}

	void Start()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		if ( rb )
			rb.freezeRotation = true;
		originalRotation = transform.localRotation;
	}

	public static float ClampAngle( float angle, float min, float max )
	{
		angle = angle % 360;
		if ( ( angle >= -360F ) && ( angle <= 360F ) )
		{
			if ( angle < -360F )
			{
				angle += 360F;
			}
			if ( angle > 360F )
			{
				angle -= 360F;
			}
		}
		return Mathf.Clamp( angle, min, max );
	}
}