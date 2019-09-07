using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidneyPalette
{

    /*
    black    #534b4b
    brown  #ae5e3c
    red        #ff4848
    orange #ff953f
    yellow  #ffcd42
    green    #8de152
    cyan    #84e5ff
    blue       #64b6ff
    purple   #ad6ec3    
     */
    
    public static Color black = new Color(0.325f, 0.294f, 0.294f, 1.0f);
    public static Color brown = new Color(0.682f, 0.369f, 0.235f, 1.0f);
    public static Color red = new Color(1.0f, 0.282f, 0.282f, 1.0f);
    public static Color orange = new Color(1.0f, 0.584f, 0.247f, 1.0f);
    public static Color yellow = new Color(1.0f, 0.804f, 0.259f, 1.0f);
    public static Color green = new Color(0.553f, 0.882f, 0.322f, 1.0f);
    public static Color cyan = new Color(0.518f, 0.898f, 1.0f, 1.0f);
    public static Color blue = new Color(0.392f, 0.714f, 1.0f, 1.0f);
    public static Color purple = new Color(0.678f, 0.431f, 0.765f, 1.0f);

    public static Color[] AsList = new Color[]{black, brown, red, orange, yellow, green, cyan, blue, purple};

    public static Color ChooseRandom()
    {
        return AsList[Random.Range(0, AsList.Length)];
    }
}
