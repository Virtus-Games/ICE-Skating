using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    [SerializeField] Transform rightPath;
    [SerializeField] Transform leftPath;
    [SerializeField] float sphereRadius;
    [SerializeField] Color gizmosColor = Color.green;

    private Transform[] rightwayPoints;
    private List<Transform> wayPoints = new List<Transform>();


    public List<Transform> WayPoints
    {
        get { return wayPoints; }

    }

    private void Start()
    {
        rightwayPoints = rightPath.GetComponentsInChildren<Transform>();
        wayPoints.Clear();
        foreach (Transform point in rightwayPoints)
        {
            if (point != rightPath)
                wayPoints.Add(point);

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        rightwayPoints = rightPath.GetComponentsInChildren<Transform>();
        wayPoints.Clear();
        foreach (Transform point in rightwayPoints)
        {
            if(point != rightPath)
            wayPoints.Add(point);
            
        }

        for (int i = 0; i < wayPoints.Count; i++)
        {
             Vector3 pos = wayPoints[i].position;

            if (i > 0)
            {
                Vector3 previousPos = wayPoints[i - 1].position;

                Debug.DrawLine(previousPos, pos);
                Gizmos.DrawWireSphere(pos, sphereRadius);
              }
        }

    }
}

