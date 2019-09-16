using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButtonScript : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject creditCanvas;


    // ------------------------------------------
    void Start()
    {
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
        }

        if (creditCanvas != null)
        {
            creditCanvas.SetActive(false);
        }
    }

	private void Update()
	{
		// Fix weird bug on return to menu, cursor kept locking (SceneController no where in sight)
		Cursor.lockState = CursorLockMode.None;
	}


	// ------------------------------------------
	public void OnPlayClick()
    {
        SceneManager.LoadScene("Main");
    }

    // ------------------------------------------
    public void OnCreditsClick()
    {
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
        }

        if (creditCanvas != null)
        {
            creditCanvas.SetActive(true);
        }
    }

    // ------------------------------------------
    public void OnCreditsReturnClick()
    {
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
        }

        if (creditCanvas != null)
        {
            creditCanvas.SetActive(false);
        }
    }
}
