using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurningType
{
    narrowTurn,extersiveTurn
}

public enum TurnSide
{
    left, right,zero
}
public class PointManager : MonoBehaviour
{
    [SerializeField] Transform nextPoint;

    public TurningType turningType;
    public TurnSide turnSide = TurnSide.zero;
    public Transform NextPoint
    {
        get { return nextPoint; }
    }

     private BoxCollider m_boxcollider;


    private void OnDrawGizmos()
    {
        m_boxcollider = GetComponent<BoxCollider>();
        Vector3 cubeSize = m_boxcollider.size;
        
       
            Debug.DrawLine(transform.position, nextPoint.position, Color.green);
 

        Gizmos.DrawWireCube(transform.position,transform.InverseTransformVector(cubeSize));
    }
}
