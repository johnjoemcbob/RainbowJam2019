using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityWander : MonoBehaviour
{
    public static float Speed = 0.025f;

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
        float horizontal = Direction.x * Speed;
        float vertical = Direction.z * Speed;

        transform.Translate(horizontal, 0, vertical);
    }

    protected void RandomiseDirection()
	{
        Direction = new Vector3(Random.Range(-0.9f, 0.9f), 0, Random.Range(-0.9f, 0.9f));
	}

    void OnCollisionEnter(Collision collision)
    {
        RandomiseDirection();
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        //NPC is stuck in a wall...
        RandomiseDirection();
    }
}
