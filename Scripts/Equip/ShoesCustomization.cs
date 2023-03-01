
using UnityEngine;

public class ShoesCustomization : MonoBehaviour
{

    [SerializeField] private GameObject childStarted;

    private void Awake()
    {
        childStarted = transform.GetChild(0).gameObject;
    }

    public void ChangeShoes(GameObject obj)
    {
        SkinnedMeshRenderer m_mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        SkinnedMeshRenderer sourceMesh = obj.GetComponent<SkinnedMeshRenderer>();

        m_mesh.sharedMesh = sourceMesh.sharedMesh;

        m_mesh.sharedMaterial = sourceMesh.sharedMaterial;

        sourceMesh.bones = sourceMesh.bones;
        
        sourceMesh.rootBone = m_mesh.rootBone;

        
        Destroy(transform.Find(obj.name + "(Clone)").gameObject);


    }

}
