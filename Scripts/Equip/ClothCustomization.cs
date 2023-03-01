using UnityEngine;

public class ClothCustomization : MonoBehaviour
{
   public void ChangCloth( GameObject obj)
    {
        SkinnedMeshRenderer m_mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        SkinnedMeshRenderer sourceMesh = obj.GetComponentInChildren<SkinnedMeshRenderer>();

        m_mesh.sharedMesh = sourceMesh.sharedMesh;

        m_mesh.sharedMaterial = sourceMesh.sharedMaterial;

        sourceMesh.bones = sourceMesh.bones;
    }
}
