using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for shared aspects such as appearance
public class NPC : MonoBehaviour
{
	PersonInfo Data;

    [SerializeField]
    private GameObject Shirt;

    [SerializeField]
    private GameObject Hat;

    [SerializeField]
    private GameObject Sunglasses;

    [SerializeField]
    private GameObject Pin;

    // Start is called before the first frame update
    void Start()
    {
        if(Shirt == null || Hat == null || Sunglasses == null || Pin == null)
        {
            Debug.LogError("Something's wrong with your NPC prefab! Make sure the Shirt, Hat, Sunglasses and Pin objects exist.");
        }
        else
        {
            // TODO: Remove this. Data should be supplied from the overall crowd/city scene manager when spawning new NPCs.
            GenerateAppearanceFromData(PersonInfo.GenerateRandom("DEBUG"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void GenerateAppearanceFromData(PersonInfo newData)
	{
        Data = newData;

        Shirt.SetActive(Data.HasShirt);
        Hat.SetActive(Data.HasHat);
        Sunglasses.SetActive(Data.HasSunglasses);
        Pin.SetActive(Data.HasPin);

        var shirtRenderer = Shirt.GetComponentInChildren<MeshRenderer>();
        if(shirtRenderer != null)
        {
            shirtRenderer.material.color = Data.ShirtColour;
        }

        var hatRenderer = Hat.GetComponentInChildren<MeshRenderer>();
        if(hatRenderer != null)
        {
            hatRenderer.material.color = Data.HatColour;
        }

        Hat.transform.localScale *= Data.HatScale;

        var sunglassesRenderer = Sunglasses.GetComponentInChildren<MeshRenderer>();
        if(sunglassesRenderer != null)
        {
            sunglassesRenderer.material.color = Data.SunglassesColour;
        }

        Sunglasses.transform.localScale *= Data.SunglassesScale;

        var pinRenderer = Pin.GetComponentInChildren<MeshRenderer>();
        if(pinRenderer != null)
        {
            pinRenderer.material.color = Data.PinColour;
        }

        Pin.transform.localScale *= Data.PinScale;
	}

    public PersonInfo GetPersonInfo()
    {
        return Data;
    }
}
