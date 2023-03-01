using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterMovementState
{
    run, turnLeft, turnRight, idle
}

    [System.Serializable]
    public enum PowerType
    {
        POWER,
        HEALTH
    }

public class CharacterControl : MonoBehaviour
{
    #region Value

    [Header("CHARACTER VARIABLES ")]

    [SerializeField] float speed;
    [SerializeField] float sideSwipeSpeed = 1;

    [Header("Area Range ")]

    [SerializeField] Transform leftRaycastPoint;

    [SerializeField] Transform rightRaycastPoint;

    [SerializeField] float rayDistance;


    [Header("ROTATION SETTINGS")]

    [SerializeField] float extensiveTurnLerpDamp;

    [SerializeField] float narrowTurnLerpDamp;

    [SerializeField] float rotationStoppedAngle;

    [Tooltip("Karakterin ilerlediği yolda kayma meydana geldiğinde tekrar yola girdiğindeki offset değeri")]
    [SerializeField] float directionOffset = 0.1f;

    [HideInInspector]
    public CharacterMovementState movementState = CharacterMovementState.idle;
    #endregion

    #region Values 2

    ///   SCRIPT REFERENCES
    private WallManager wallManager;
    private BlendRoadManager roadManager;
    private PointManager pointManager;
    private InputManager IM;
    private AnimatorController ac;


    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform GroundPoint;
    [SerializeField]
    private float gravityForce = 2.3f;

    ///  COROUTINE INIT

    private Coroutine xOffsetCoroutine;

    /// COMPONENT REFERENCES

    private CharacterController cc;

    /// PRIVATE FIELDS

    private TurnSide turnSide = TurnSide.zero;

    private bool isTurning = false;

    private float lerpY = 90;

    private Vector3 nextPoint;

    private bool isCrushTheTarget = false;

    private bool stopLeftSide;

    private bool stopRightSide;

    public GameObject[] skates;

    public TurnSide TurnSide
    {
        get { return turnSide; }
    }

    public bool IsCrushTheTarget
    {
        get { return isCrushTheTarget; }
    }
    private Coroutine collect;
    private Coroutine posAnim;

    MarketSystem marketSystem;

    #endregion

    #region Value 3
    private TextMeshProUGUI CollectText;
    private Image Status;
    float num;
    int goodValue;
    int badValue;
    float end;

    float runSpeed;
    [SerializeField] private float gravity = 9.81f;
    private string[] goodAdvice = {
    "Good",
    "Come on",
    "Your good",
    "Best",
    "All right",
    "Lest Go!",
    "Best way",
    "Dancer"
    };
    private string[] badAdvince = {
    "No",
    "Be Careful",
    "not good",
    "Come on",
    "you can it",
    "Lest break!",
    "no way",
    ""
    };

    public bool isStop = false;
    [SerializeField] private ParticleSystem stoeinEffect;
    [SerializeField] private ParticleSystem powerEffect;
    [SerializeField] private ParticleSystem healthEffect;

    public float jumpSpeed;
    bool isJump = false;
    Animator m_anim;
    float ySpeed = 0;

    #endregion


    public void PowerUpdate(PowerType type)
    {
        if (type == PowerType.POWER)
        {
            if (powerEffect.isPlaying)
                powerEffect.Stop();
            powerEffect.Play();
        }
        else if (type == PowerType.HEALTH)
        {
            if (healthEffect.isPlaying)
                healthEffect.Stop();
            healthEffect.Play();
        }
    }


    private void Start()
    {
        marketSystem = GameObject.FindWithTag("GameManager").GetComponent<MarketSystem>();
        GetSpped();
        Starting();
        IM = FindObjectOfType<InputManager>();
        Debug.Log(IM.gameObject == null ? "" : "Not wodasd");
        cc = GetComponent<CharacterController>();
        ac = GetComponent<AnimatorController>();
        m_anim = GetComponent<Animator>();

    }

    void Starting()
    {
        CollectText = GameObject.FindWithTag("CollectText").GetComponent<TextMeshProUGUI>();
        CollectText.text = "";
        Status = GameObject.FindWithTag("CollectImage").GetComponent<Image>();
    }
    #region Move

    void CheckGround()
    {


        if (!Physics.CheckSphere(GroundPoint.position, .5f, ~playerLayer))
            transform.position -= new Vector3(0, gravityForce * Time.deltaTime, 0);

        //gravity -= 0.2f * Time.deltaTime;
        //cc.Move(new Vector3(transform.position.x, gravity, transform.position.z));
        //if (cc.isGrounded) gravity = 0;

        if (isJump)
            ySpeed = jumpSpeed;

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (cc.isGrounded)
            ySpeed = 0f;

        if (transform.position.y > 0.3 || !cc.isGrounded)
            cc.Move(transform.up * Time.deltaTime * -10f);


    }
    void CharacterMovement()
    {
        // x ekseninindeki sapmanın düzeltilmesi
        // cc.Move((transform.forward + transform.right * sideWayDistance/7 ) * speed * Time.deltaTime);

        if ((!stopRightSide && IM.sideDirection == SideDirection.right) || (!stopLeftSide && IM.sideDirection == SideDirection.left))
            cc.Move(transform.forward * speed * Time.deltaTime);
        else
            cc.Move(transform.forward * speed * Time.deltaTime + (transform.right * IM.DifferenceX).normalized * Time.deltaTime * sideSwipeSpeed);
    }
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
    private void AreaRange()
    {
        if (Physics.Raycast(rightRaycastPoint.position, -rightRaycastPoint.transform.up, out RaycastHit rightRaycast, rayDistance))
        {
            stopRightSide = true;
        }
        else
        {
            stopRightSide = false;
        }

        if (Physics.Raycast(leftRaycastPoint.position, -leftRaycastPoint.transform.up, out RaycastHit leftRaycast, rayDistance))
        {
            stopLeftSide = true;
        }
        else
        {
            stopLeftSide = false;
        }
    }

    /*
    IEnumerator Xoffset()
    {
        while (true)
        {

            if (roadManager.BlenderRoadEulerAnglesY == 270)
            {
                sideWayDistance = transform.position.x - nextPoint.x;
            }

            else if (roadManager.BlenderRoadEulerAnglesY == 90)
            {
                sideWayDistance = transform.position.z - nextPoint.z;
            }

            if (Mathf.Abs(sideWayDistance) < directionOffset)
            {
                sideWayDistance = 0;
                break;
            }
            yield return null;
        }
    }
    */
    private void OnDrawGizmos()
    {

        Debug.DrawRay(rightRaycastPoint.position, -rightRaycastPoint.up * rayDistance);
        Debug.DrawRay(leftRaycastPoint.position, -leftRaycastPoint.up * rayDistance);
    }
    #endregion


    private void Update()
    {

        if (!isStop || Input.touchCount > 0)
        {
            PlayerRotation();
            CharacterMovement();
            AreaRange();
        }

        CheckGround();
        // message();

    }
    public float GetSpped()
    {
        speed = PlayerPrefs.GetInt("speedcontrol");
        return speed;
    }
    public IEnumerator IncrementSpeed(int val)
    {
        speed += val;
        yield return new WaitForSeconds(5f);
        speed = GetSpped();
    }

    #region Status and Envoriments

    void SkatesController(GameObject other, string name, int val)
    {
        stoeinEffect.gameObject.SetActive(true);
        stoeinEffect.Play();
        if (other.gameObject.name == name)
        {
            foreach (GameObject obj in skates)
            {
                obj.SetActive(false);
            }

            skates[val].SetActive(true);
            Destroy(other.gameObject);
            stoeinEffect.gameObject.SetActive(false);

        }
    }

    public void UpdateCollect(int collect, int status)
    {

        if (collect < 0)
        {
            goodValue = 0;

            if (badValue < badAdvince.Length - 1)
                badValue += 1;
            else
                badValue = 0;

            if (Status.fillAmount != 0)
                StartCoroutine(CollectTime(badAdvince, badValue));

            //TitleControl(badValue, badAdvince.Length, 0, badAdvince);
        }
        else
        {
            badValue = 0;
            if (goodValue < goodAdvice.Length - 1)
                goodValue += 1;
            else
                goodValue = 0;

            if (Status.fillAmount != 1)
                StartCoroutine(CollectTime(goodAdvice, goodValue));

            //TitleControl(goodValue, goodAdvice.Length, 1, goodAdvice);
        }

        UpdateStatus(status);
        CollectText.GetComponent<Animator>().SetBool("isBig", true);
    }

    IEnumerator CollectTime(string[] val, int ins)
    {
        yield return new WaitForSeconds(0.2f);
        CollectText.text = val[ins];
        yield return new WaitForSeconds(1f);
        CollectText.text = "";
    }

    void UpdateStatus(float val)
    {

        if (num > 1)
            num = 1;
        else if (num < 0)
            num = 0;

        num += val;
        Status.fillAmount += (num / 100);

        if (Status.fillAmount < 0.3f)
            ColorControl(Color.red);
        else if (Status.fillAmount >= 0.3 && Status.fillAmount <= 0.7f)
            ColorControl(Color.yellow);
        else
            ColorControl(Color.green);


    }
    public float StatusImage()
    {
        return Status.fillAmount;
    }
    void ColorControl(Color col)
    {
        Status.color = Color.Lerp(Status.color, col, 8f);
    }
    IEnumerator DestroySnowMan(GameObject obj)
    {

        yield return new WaitForSeconds(0.2f);
        Destroy(obj);
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramba"))
        {
            isStop = true;
            // Vector3 hdf =  other.gameObject.transform.Find("Hedef").localPosition;
            // Vector3 pos = new Vector3(transform.position.x - 1, hdf.y, hdf.z);
            // other.gameObject.transform.Find("Hedef").position = pos;

            gameObject.AddComponent<ParabolaController>().OpenPanel(other.gameObject);
        }

        if (other.gameObject.CompareTag("Pos"))
        {
            GetComponent<AnimatorController>().
                PosController(
                    other.gameObject.
                        GetComponent<AnimationPosController>().GetClips());

            UpdateCollect(1, 9);
            GameManager.Current.ChangeScore(4);
        }

        if (other.gameObject.CompareTag("Skates"))
        {
            Debug.Log("Skates");
            other.GetComponentInChildren<ParticleSystem>().Play();
            GameManager.Current.ChangeScore(2);
            UpdateCollect(1, 7);

            StartCoroutine(DestroySnowMan(other.gameObject));
        }

        if (other.gameObject.CompareTag("snow"))
        {
            GameManager.Current.ChangeScore(4);
            UpdateCollect(1, 2);
            other.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(DestroySnowMan(other.gameObject));

        }

        if (other.gameObject.CompareTag("SnowMan"))
        {
            GameManager.Current.ChangeScore(-2);
            UpdateCollect(-1, -2);
            other.GetComponent<SnowmanController>().rbActived();
            other.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(DestroySnowMan(other.gameObject));
        }

        if (other.CompareTag("target"))
            ac.BeginCoroutine();

        if (other.GetComponent<PointManager>() != null)
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
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            GetComponent<AnimatorController>().StopFinish();
            Camera.main.GetComponentInParent<SoftCameraControl>().enabled = false;
            isStop = true;
        }
    }
    public void RotationMove()
    {
        ac.isMove = false;
        ac.AnimControl("isMove", false);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, Quaternion.Euler(new Vector3(0, -135, 0)),
            Time.deltaTime * 5f);
    }

}





