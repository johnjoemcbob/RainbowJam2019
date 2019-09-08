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
