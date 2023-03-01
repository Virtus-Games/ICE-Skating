using System.Collections;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private Transform MainPoint, MarketPoint, RunPoint;
    [Range(0, 50)] [SerializeField] private float speedLerp = 2;
    [Range(0, 50)] [SerializeField] private float rotationLerp = 2;
    private bool _isCompetedVector;
    private Transform Target;
    void Start()
    {
        ChangeTransform(MainPoint);
    }

    private void OnEnable()
    {
        GetComponent<SoftCameraControl>().enabled = false;
    }


    private void OnDisable()
    {
        Target = null;
        GetComponent<SoftCameraControl>().enabled = true;
    }

    public void ChangeTransform(Transform pos)
    {
       if(pos != null)
        {
            _isCompetedVector = true;
            StartCoroutine(ChangeController(pos));
        }
    }

    IEnumerator ChangeController(Transform pos)
    {
        Target = pos;
        while (_isCompetedVector)
        {
            transform.position = Vector3.Slerp(transform.position, Target.position, speedLerp * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, rotationLerp * Time.deltaTime);
            if (Vector3.Distance(Target.position, transform.position) <= 0.05f)
                _isCompetedVector = false;

            yield return null;
        }
    }


}
