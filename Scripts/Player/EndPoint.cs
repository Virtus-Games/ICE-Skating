using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] Vector3 cubeSize;

    
    public Vector3 CubeSize
    {
        set { cubeSize = value; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,cubeSize);
    }
}
