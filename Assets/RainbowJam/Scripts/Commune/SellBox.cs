using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellBox : MonoBehaviour
{
	public static SellBox Instance;

	public const int Price = 5;

	public Text MoneyText;

	[HideInInspector]
	public int Money = 0;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void AddMoney( int add )
	{
		Money += add;
		MoneyText.text = "$" + Money.ToString();
	}
	
	public void SellJam( int count )
	{
		AddMoney( count * Price );
	}
}
