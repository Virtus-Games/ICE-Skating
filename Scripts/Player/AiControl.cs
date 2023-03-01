    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;




    public enum SideCheck
    {
        left, right, middle
    }
    public class AiControl : MonoBehaviour
    {
        public SideCheck sideCheck;

        private CharacterController cc;



        [Header("ROTATION SETTINGS")]

        [SerializeField] float extensiveTurnLerpDamp;

        [SerializeField] float narrowTurnLerpDamp;

        [SerializeField] float rotationStoppedAngle;

        ///   SCRIPT REFERENCES
        private WallManager wallManager;
        private BlendRoadManager roadManager;
        private PointManager pointManager;

        /// PRIVATE FIELDS

        private TurnSide turnSide = TurnSide.zero;
        private bool isTurning = false;

        private float lerpY = 90;

        private Vector3 nextPoint;



        private bool stopLeftSide;

        private bool stopRightSide;

        [SerializeField]
        private float forwardRayDistance;

        [SerializeField]
        private float groundRayDistance;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float sideWaySpeed;

        [SerializeField]
        LayerMask playerLayer;

        [SerializeField]
        Transform[] forwardRayPoints;

        [SerializeField]
        Transform[] groundRayPoints;

        [SerializeField]
        Transform barrierRay;

        private Vector3 targetPos = Vector3.zero;

        private Vector3 targetDir;



        private bool isDetectTarget = false;

        private bool isDetectBarrier = false;







        private void Start()
        {
            cc = GetComponent<CharacterController>();


        }

        private void Update()
        {
            PlayerRotation();

            TargetRaycastControl();

            GroundRayControl();

            BarrierRaycastControl();

            CharacterMove();



        }

       //KARAKTER DÖNÜŞLERDEKİ DÖNME NOKTALARINA ÇARPTIĞINDA ALGILANACAK 
        private void OnTriggerEnter(Collider other)
        {

            if (!other.CompareTag("target") && !other.CompareTag("barrier"))
            {
                lerpY = transform.rotation.eulerAngles.y;
                nextPoint = other.GetComponent<PointManager>().NextPoint.position;
                wallManager = other.GetComponent<WallManager>();
                pointManager = other.GetComponent<PointManager>();
                roadManager = other.transform.GetComponentInParent<BlendRoadManager>();
                turnSide = pointManager.turnSide;


                isTurning = true;
            }


        }


        void CharacterMove()
        {
            if (isDetectTarget)
            {
                cc.Move(targetDir * Time.deltaTime * sideWaySpeed);


                if (Vector3.Distance(transform.position, targetPos) < .5f)
                {
                    isDetectTarget = false;
                }
            }

            else
            {
                cc.Move(transform.forward * Time.deltaTime * speed);

            }

        }


        void CharacterEscapeTheBarrier(SideCheck sideCheck)
        {
            if (sideCheck == SideCheck.left)
            {
                cc.Move(transform.right * Time.deltaTime * sideWaySpeed);
            }
            else if (sideCheck == SideCheck.right)
            {
                cc.Move(-transform.right * Time.deltaTime * sideWaySpeed);
            }
        }

        //TOPLANACAK OLAN HEDEFLERİN BELİRLENMESİ İÇİN GEREKEN RAYCASTLER
        void TargetRaycastControl()
        {
            foreach (var ray in forwardRayPoints)
            {
                if (Physics.Raycast(ray.position, ray.forward, out RaycastHit hit, forwardRayDistance, ~playerLayer) && !isDetectTarget)
                {
                    if (hit.collider.CompareTag("target"))
                    {
                        targetPos = hit.collider.transform.position;
                        targetDir = (targetPos - transform.position).normalized;
                        isDetectTarget = true;
                    }
                }

                Debug.DrawRay(ray.position, ray.forward * forwardRayDistance);
            }
        }

        //KAÇILMASI GEREKEN HEDEFLERİN BELİRLENMESİ İÇİN GEREKEN RAYCASTLER
        void BarrierRaycastControl()
        {
            if (Physics.Raycast(barrierRay.position, barrierRay.forward, out RaycastHit hit, forwardRayDistance, ~playerLayer))
            {
                if (hit.collider.CompareTag("barrier"))
                {
                    isDetectBarrier = true;
                }
            }
            Debug.DrawRay(barrierRay.position, barrierRay.forward * forwardRayDistance, Color.red);
        }

         //kAÇILACAK OLAN HEDEF ALGILANDIĞI ZAMAN KARAKTERİN YOL ÜZERİNDEKİ KONUMUNA GÖRE SAĞA VEYA SOLA KAÇMASI İÇİN GEREKEN KISIM
        void GroundRayControl()
        {

            if (isDetectBarrier)
            {
                bool leftRay = Physics.Raycast(groundRayPoints[0].position, -groundRayPoints[0].up, out RaycastHit leftHit, groundRayDistance);
                bool rightRay = Physics.Raycast(groundRayPoints[1].position, -groundRayPoints[1].up , out RaycastHit rightHit, groundRayDistance);

                if (leftRay)
                {
                    sideCheck = SideCheck.right;
                    CharacterEscapeTheBarrier(sideCheck);

                }
                if (rightRay)
                {
                    sideCheck = SideCheck.left;
                    CharacterEscapeTheBarrier(sideCheck);

                }

                if (!leftRay && !rightRay)
                {
                    sideCheck = SideCheck.middle;
                    isDetectBarrier = false;
                }
            }

        Debug.DrawRay(groundRayPoints[0].position, -groundRayPoints[0].up * groundRayDistance,Color.magenta);
        Debug.DrawRay(groundRayPoints[1].position, -groundRayPoints[1].up * groundRayDistance,Color.magenta);

        }

        //VİRAJLARDA DÖNMESİ İÇİN GEREKEN KISIM
        void PlayerRotation()
        {
            if (isTurning)
            {

                float distance = Vector3.Distance(transform.position, nextPoint);


                if (pointManager.turningType == TurningType.extersiveTurn)
                {
                    lerpY = Mathf.Lerp(lerpY, wallManager.RotateValue, Time.deltaTime * extensiveTurnLerpDamp / distance);

                }

                else if (pointManager.turningType == TurningType.narrowTurn)
                {
                    lerpY = Mathf.Lerp(lerpY, wallManager.RotateValue, Time.deltaTime * narrowTurnLerpDamp / distance);

                }


                if (Mathf.Abs(transform.eulerAngles.y - wallManager.RotateValue) < rotationStoppedAngle)
                {
                    transform.rotation = Quaternion.Euler(0, wallManager.RotateValue, 0);
                    isTurning = false;


                    #region Offset fixing 
                    //if (xOffsetCoroutine != null)
                    //{
                    //    StopCoroutine(Xoffset());
                    //}
                    //xOffsetCoroutine = StartCoroutine(Xoffset());
                    #endregion

                    return;
                }
                transform.rotation = Quaternion.Euler(0, lerpY, 0);

            }
        }



    }
