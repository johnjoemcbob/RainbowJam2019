using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityWander : MonoBehaviour
{
    public static float Speed = 0.025f;

	protected Vector3 StartPos;
	protected Vector3 Target;
	protected Vector3 Direction;

    private enum FaceDir { UNKNOWN, LEFT, RIGHT }
    private FaceDir currentDir = FaceDir.UNKNOWN;

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
        UpdateFacing();
	}

    protected void UpdateFacing()
    {
        Transform walkingSprite = transform.Find("BaseCharacter/Walking/WalkingSprite");

        if (walkingSprite == null)
            return;

        Vector3 normDir = Direction;
        normDir.Normalize();

        FaceDir tempDir = FaceDir.LEFT;

        //Figure out which way we're facing
        if (normDir.x < 0.0f)
        {
            tempDir = FaceDir.RIGHT;
        }


        //Flip here
        if (currentDir != tempDir)
        {
            if (tempDir == FaceDir.LEFT)
            {
                //LEFT
                walkingSprite.localEulerAngles = new Vector3(0, 0, 0);
                // Update z offset of all children to be in front of animation
                foreach (Transform child in walkingSprite.transform)
                {
                    child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, Mathf.Abs(child.localPosition.z));
                }
            }
            else
            {
                //RIGHT
                walkingSprite.localEulerAngles = new Vector3(0, 180, 0);
                // Update z offset of all children to be in front of animation
                foreach (Transform child in walkingSprite.transform)
                {
                    child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, -Mathf.Abs(child.localPosition.z));
                }
            }

            currentDir = tempDir;
        }
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
