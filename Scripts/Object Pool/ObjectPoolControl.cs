using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolControl : MonoBehaviour
{

    ObjectPooling objectPoolingManager;
    Queue<GameObject> lastObjects = new Queue<GameObject>();
    private void Start()
    {
        objectPoolingManager = GetComponent<ObjectPooling>();
    }

    GameObject cube;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            cube =  objectPoolingManager.GetObjectPooling(0);
            lastObjects.Enqueue(cube);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cube = lastObjects.Dequeue();
            objectPoolingManager.SetObjectPool(cube, 0);

        }
    }
}
