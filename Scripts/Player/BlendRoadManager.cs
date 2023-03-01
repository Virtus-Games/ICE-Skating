using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlendRoadManager : MonoBehaviour
{

    [TextArea(8, 8)]
    public string roadDegreeNotice;


    [SerializeField] Transform rightWayStartPoint;

    [SerializeField] Transform rightWayEndPoint;

    [SerializeField] Transform leftWayStartPoint;

    [SerializeField] Transform leftWayEndPoint;

    [SerializeField] bool invert = false;
    private float blenderRoadEulerAnglesY;

    public float BlenderRoadEulerAnglesY
    {
        get { return blenderRoadEulerAnglesY; }
    }


    //[SerializeField] Vector3 rightWayStartPointOffset;


    //[SerializeField] Vector3 rightWayEndPointOffset;


    //[SerializeField] Vector3 lefttWayStartPointOffset;

    //[SerializeField] Vector3 leftWayEndPointOffset;


    private void Start()
    {
        blenderRoadEulerAnglesY = transform.eulerAngles.y;
    }


    private Transform rightStartPoint;

    private Transform rightEndPoint;

    private Transform leftStartPoint;

    private Transform leftEndPoint;

    private Transform connectPoint1;

    private Transform connectPoint2;



    private void OnDrawGizmos()
    {

        rightStartPoint = transform.GetChild(0);
        rightEndPoint = transform.GetChild(1);

        leftStartPoint = transform.GetChild(2);
        leftEndPoint = transform.GetChild(3);


        if (rightWayStartPoint && rightWayEndPoint && invert && transform.eulerAngles.y == 90)
        {
            rightWayStartPoint.position = rightStartPoint.position + new Vector3(.5f, 0, 0);

            rightWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

            rightWayEndPoint.position = rightEndPoint.position + new Vector3(0, 0, 0.5f);

            rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);
        }  

        if (rightWayStartPoint && rightWayEndPoint && invert && transform.eulerAngles.y == 270)
        {
            rightWayStartPoint.position = leftEndPoint.position + new Vector3(0, 0, -.5f);

            rightWayStartPoint.localRotation = Quaternion.Euler(0, 90, 0);

            rightWayEndPoint.position = leftStartPoint.position + new Vector3(-.5f, 0, 0);

            rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);
        }

        if (rightWayStartPoint && rightWayEndPoint && invert && transform.eulerAngles.y == 180)
        {
            rightWayStartPoint.position = rightStartPoint.position + new Vector3(0, 0, -.5f);

            rightWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

            rightWayEndPoint.position = rightEndPoint.position + new Vector3(0.5f, 0, 0f);

            rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);
        } 
        
        
        if (rightWayStartPoint && rightWayEndPoint && invert && transform.eulerAngles.y == 0)
        {
            rightWayStartPoint.position = leftEndPoint.position + new Vector3(-0.5f, 0, 0);

            rightWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

            rightWayEndPoint.position = leftStartPoint.position + new Vector3(0, 0, 0.5f);

            rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);
        }

        if (rightWayStartPoint && rightWayEndPoint && !invert)
        {
            if (transform.eulerAngles.y == 0)
            {
                rightWayStartPoint.position = rightStartPoint.position + new Vector3(0, 0, .5f);

                rightWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

                rightWayEndPoint.position = rightEndPoint.position + new Vector3(-.5f, 0, 0);

                rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);
            }

            if (transform.eulerAngles.y == 90)
            {
                rightWayStartPoint.position = leftEndPoint.position + new Vector3(0, 0, .5f);

                rightWayStartPoint.localRotation = Quaternion.Euler(0, 90, 0);

                rightWayEndPoint.position = leftStartPoint.position + new Vector3(.5f, 0, 0);

                rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);
            }

            if (transform.eulerAngles.y == 180)
            {
                rightWayStartPoint.position = leftEndPoint.position + new Vector3(.5f, 0, 0);

                rightWayStartPoint.localRotation = Quaternion.Euler(0, 90, 0);

                rightWayEndPoint.position = leftStartPoint.position + new Vector3(0, 0, -.5f);

                rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);
            }
            if (transform.eulerAngles.y == 270)
            {
                rightWayStartPoint.position = rightStartPoint.position + new Vector3(-.5f, 0, 0);

                rightWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

                rightWayEndPoint.position = rightEndPoint.position + new Vector3(0, 0, -.5f);

                rightWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);
            }





        }
        if (leftWayStartPoint && leftWayEndPoint && invert && transform.eulerAngles.y ==0)
        {
            leftWayStartPoint.position = rightEndPoint.position + new Vector3(-.5f, 0, 0);

            leftWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

            leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);

            leftWayEndPoint.position = rightStartPoint.position + new Vector3(0, 0, 0.5f);
        }

        if (leftWayStartPoint && leftWayEndPoint && invert && transform.eulerAngles.y == 90)
        {
            leftWayStartPoint.position = leftStartPoint.position + new Vector3(.5f, 0, 0);

            leftWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

            leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);

            leftWayEndPoint.position = leftEndPoint.position + new Vector3(0, 0, 0.5f);
        }

        if (leftWayStartPoint && leftWayEndPoint && invert && transform.eulerAngles.y == 180)
        {
            leftWayStartPoint.position = leftStartPoint.position + new Vector3(0, 0, -.5f);

            leftWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

            leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);

            leftWayEndPoint.position = leftEndPoint.position + new Vector3(0.5f, 0, 0);
        }
        
        if (leftWayStartPoint && leftWayEndPoint && invert && transform.eulerAngles.y == 270)
        {
            leftWayStartPoint.position = rightEndPoint.position + new Vector3(0, 0, -.5f);

            leftWayStartPoint.localRotation = Quaternion.Euler(0, 90, 0);

            leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);

            leftWayEndPoint.position = rightStartPoint.position + new Vector3(-0.5f, 0, 0);
        }

        if (leftWayStartPoint && leftWayEndPoint && !invert)
        {
            if (transform.eulerAngles.y == 0)
            {
                leftWayStartPoint.position = leftStartPoint.position + new Vector3(0, 0, .5f);

                leftWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

                leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);

                leftWayEndPoint.position = leftEndPoint.position + new Vector3(-.5f, 0, 0);
            }

            if (transform.eulerAngles.y == 90)
            {
                leftWayStartPoint.position = rightEndPoint.position + new Vector3(0, 0, .5f);

                leftWayStartPoint.localRotation = Quaternion.Euler(0, 90, 0);

                leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(1, 5, 5.399f);

                leftWayEndPoint.position = rightStartPoint.position + new Vector3(.5f, 0, 0);
            }

            if (transform.eulerAngles.y == 180)
            {
                leftWayStartPoint.position = rightEndPoint.position + new Vector3(.5f, 0, 0);

                leftWayStartPoint.localRotation = Quaternion.Euler(0, 90, 0);

                leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);

                leftWayEndPoint.position = rightStartPoint.position + new Vector3(0, 0, -.5f);
            }
            if (transform.eulerAngles.y == 270)
            {
                leftWayStartPoint.position = leftStartPoint.position + new Vector3(-.5f, 0, 0);

                leftWayStartPoint.localRotation = Quaternion.Euler(0, 0, 0);

                leftWayEndPoint.GetComponent<EndPoint>().CubeSize = new Vector3(5.399f, 5, 1);

                leftWayEndPoint.position = leftEndPoint.position + new Vector3(0, 0, -.5f);
            }


        }


    }
}
