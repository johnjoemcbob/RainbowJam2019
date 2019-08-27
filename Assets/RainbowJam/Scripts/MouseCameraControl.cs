using UnityEngine;

[AddComponentMenu( "Camera-Control/Mouse" )]
public class MouseCameraControl : MonoBehaviour
{
	[System.Serializable]
	// Handles common parameters for translations and rotations
	public class MouseControlConfiguration
	{
		public float sensitivity;
	}

	[System.Serializable]
	// Handles scroll parameters
	public class MouseScrollConfiguration
	{
		public bool activate;
		public float sensitivity;
	}

	// Yaw default configuration
	public MouseControlConfiguration yaw = new MouseControlConfiguration { sensitivity = 10F };

	// Pitch default configuration
	public MouseControlConfiguration pitch = new MouseControlConfiguration { sensitivity = 10F };

	// Scroll default configuration
	public MouseScrollConfiguration scroll = new MouseScrollConfiguration { sensitivity = 2F };

	// Default unity names for mouse axes
	public string mouseHorizontalAxisName = "Mouse X";
	public string mouseVerticalAxisName = "Mouse Y";
	public string scrollAxisName = "Mouse ScrollWheel";

	void LateUpdate()
	{
		float rotationX = Input.GetAxis(mouseHorizontalAxisName) * yaw.sensitivity;
		transform.localEulerAngles += new Vector3( 0, rotationX, 0 );

		float rotationY = Input.GetAxis(mouseVerticalAxisName) * pitch.sensitivity;
		transform.localEulerAngles += new Vector3( -rotationY, 0, 0 );

		// TODO
		float translateZ = Input.GetAxis(scrollAxisName) * scroll.sensitivity;
		transform.Translate( 0, 0, translateZ );
	}
}
