using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPosController : MonoBehaviour
{
    [SerializeField] private string tag;
    
    public string GetClips()
    {
        return tag;
    }


}
