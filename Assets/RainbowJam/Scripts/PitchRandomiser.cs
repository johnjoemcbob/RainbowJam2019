using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchRandomiser : MonoBehaviour
{
	public Vector2 Range;

	protected AudioSource Source;

	private void Start()
	{
		Source = GetComponent<AudioSource>();
	}

	void Update()
    {
		Source.pitch = Random.Range( Range.x, Range.y );
    }
}
