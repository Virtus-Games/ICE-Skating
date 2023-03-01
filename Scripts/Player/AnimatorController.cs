using System.Collections;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator m_animator;

    private CharacterControl cc;

    private bool isPose = false;

    private float moveValue = 0;

    private float poseValue = 0;

    private string[] poses = { "pose1", "pose2" };

    private Coroutine poseAnim;
    private Coroutine poseAnim_2;

    [SerializeField]
    float moveValueEnterpolationSpeed = 1f;

    [SerializeField]
    float poseValueEnterpolationSpeed = 1f;

    float pose2Value;
    string posedName;

    Transform endPosition;

    public bool isMove = false;
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        cc = GetComponent<CharacterControl>();
        endPosition = GameObject.FindWithTag("End").transform;
    }

    public void SetMoveEnd()
    {
        StartCoroutine(GoThere(endPosition.position));
    }

    bool isOk = false;
    IEnumerator GoThere(Vector3 pos)
    {
        while (!isOk)
        {
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * 3f);

            if (Vector3.Distance(pos, transform.position) <= 0.1f)
            {
                isOk = true;
                Camera.main.GetComponentInParent<SoftCameraControl>().enabled = false;
            }
            yield return null;
        }
    }

    public void StopFinish()
    {
        GameManager.Current.Juri();
        isMove = false;
        cc.enabled = false;
        m_animator.SetBool("isIdle", true);
        m_animator.SetBool("isMove", false);
        StartCoroutine(Wait());

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<LevelManager>().Win();
    }

    public void AnimControl(string val, bool isOk)
    {
        m_animator.SetBool(val, isOk);
    }

    public void RunPlayer()
    {
        AnimControl("isMove", true);
        isMove = true;
        gameObject.GetComponent<CharacterControl>().enabled = true;
        Debug.Log("here");
    }
    void MoveAnim()
    {
        // && Input.touchCount > 0
        if (!isPose && isMove)
        {
            moveValue = Mathf.Lerp(moveValue, 1,
                 Time.deltaTime * moveValueEnterpolationSpeed);
            m_animator.SetFloat("velocityY", moveValue);
        }
        else
            m_animator.SetFloat("velocityY", -1, 2f, Time.deltaTime);
    }

    private void Update()
    {
        MoveAnim();
    }

    public void BeginCoroutine()
    {
        if (poseAnim != null)
        {
            StopCoroutine(PoseAnim());
        }

        poseAnim = StartCoroutine(PoseAnim());
    }
    public void PosController(string name)
    {
        isPose = true;

        m_animator.SetFloat("velocityY", 0);

        posedName = name;

        if (poseAnim_2 != null)
            StopCoroutine(PosEnumarator());

        poseAnim_2 = StartCoroutine(PosEnumarator());

    }
    IEnumerator PosEnumarator()
    {
        StartCoroutine(cc.IncrementSpeed(2));

        while (pose2Value < .95f)
        {
            pose2Value = Mathf.Lerp(pose2Value, 1, Time.deltaTime * 2);
        }

        m_animator.SetFloat(posedName, pose2Value);

        yield return new WaitForSeconds(0.5f);

        pose2Value = 0;

        m_animator.SetFloat(posedName, 0);

        isPose = false;

    }
    IEnumerator PoseAnim()
    {
        isPose = true;

        moveValue = 0;

        while (poseValue < .95f)
        {
            poseValue = Mathf.Lerp(poseValue, 1f, Time.deltaTime * poseValueEnterpolationSpeed);
        }

        int rand = Random.Range(0, 2);

        m_animator.SetFloat(poses[rand], poseValue);

        m_animator.SetFloat("velocityY", moveValue);

        yield return new WaitForSeconds(1.5f);

        isPose = false;

        poseValue = 0;

        m_animator.SetFloat("pose1", poseValue);

    }

}
