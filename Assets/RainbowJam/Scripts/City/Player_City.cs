using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_City : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * 0.5f;
        float vertical = Input.GetAxis("Vertical") * speed;

        transform.Translate(-horizontal, 0, -vertical);
    }
}
