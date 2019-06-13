using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x != target.position.x)
            transform.position = new Vector3(target.position.x + 3f, transform.position.y, target.position.z);

        if (transform.position.z != target.position.z)
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
    }
}
