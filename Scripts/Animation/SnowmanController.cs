using UnityEngine;

public class SnowmanController : MonoBehaviour
{

    public void rbActived()
    {
        Rigidbody[] rbs = transform.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rbActived();

        }
    }
}
