using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZone : MonoBehaviour
{

    public string[] TutorialText;
        
    private Vector3 originPoint;

    [SerializeField]
    private BoxCollider zoneCollider;

    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = originPoint + new Vector3(0.0f, Mathf.Sin((float)Time.frameCount*0.05f) * 0.1f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Did the player touch this?
        if(other.gameObject.name.ToLower().Contains("player"))
        {
            OnPlayerCollided();
        }
    }

    void OnPlayerCollided()
    {
        for(int i = 0; i < TutorialText.Length; i++)
        {
            SceneController.Instance.SummonDialogueBubble(TutorialText[i]);
        }

        GameObject.Destroy(this.gameObject); //pop!
    }
}
