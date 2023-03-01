using UnityEngine;


public enum SideDirection
{
    left, right, stop
}


public class InputManager : MonoBehaviour
{

    private float differenceX;

    private float bestLargeX;
    private float currentLargeX;

    private float currentX;


    /// ENUM INITIATION
    [HideInInspector]
    public SideDirection sideDirection = SideDirection.stop;


    public float DifferenceX
    {
        get { return differenceX; }
    }



    private void Update()
    {



        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {

                bestLargeX = currentX;
                currentX = touch.position.x;

                if (bestLargeX < currentX)
                {
                    differenceX = 1;
                    sideDirection = SideDirection.right;
                }
                else if (bestLargeX > currentX)
                {
                    differenceX = -1;
                    sideDirection = SideDirection.left;
                }
                else
                {
                    differenceX = 0;
                    sideDirection = SideDirection.stop;
                }

            }
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Stationary)
            {
                differenceX = 0;
                sideDirection = SideDirection.stop;
            }
        }
    }






        //#if UNITY_EDITOR

        //        Vector3 mousePose = Input.mousePosition;
        //        Vector3 playerPose = cam.WorldToScreenPoint(ncc.transform.position);

        //        float warp = (mousePose.x - playerPose.x) / warpRate;
        //        print(warp);

        //        if (Input.GetMouseButton(0))
        //        {
        //            if (warp > 2)
        //            {
        //                 differenceX = Mathf.Lerp(differenceX, 1, Time.deltaTime);

        //            }
        //            if (warp < -2)
        //            {
        //                differenceX = Mathf.Lerp(differenceX,-1,Time.deltaTime);
        //            }
        //            else if (warp > -2 && warp < 2)
        //            {
        //                differenceX = 0;
        //            }

        //        }
        //        if (Input.GetMouseButtonUp(0))
        //        {
        //            differenceX = 0;
        //        }



        //#endif

    
}




