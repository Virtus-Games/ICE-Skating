using UnityEngine;

public class SoftCameraControl : MonoBehaviour
{

    [Header("CAMERA SETTINGS")]
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] Vector3 cameraOffset_Finish;
    [SerializeField] Vector3 cameraRotationOffset;
    [SerializeField] float followSpeed;

    private Transform target;
    private Camera cam;
    public bool isFollow = false;

    private void OnEnable()
    {
        target = GameObject.FindWithTag("Player").transform;
        cam = Camera.main;
    }

    private void OnDisable()
    {
        cam.transform.localPosition = cameraOffset + cameraOffset_Finish;
    }


    private void Update()
    {

        transform.position = Vector3.Slerp(transform.position,
            target.position, followSpeed * Time.deltaTime);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
            target.eulerAngles + cameraRotationOffset, Time.deltaTime * followSpeed);
        cam.transform.localPosition = cameraOffset;
    }





}
