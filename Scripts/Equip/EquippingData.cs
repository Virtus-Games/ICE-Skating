using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName ="Costume" , menuName ="Scriptable/Costume",order = 1)]
public class EquippingData : ScriptableObject
{

    public List<Costume> costumes;

  
    [Serializable]
    public struct Costume
    {
         public GameObject obj;


    }
}
