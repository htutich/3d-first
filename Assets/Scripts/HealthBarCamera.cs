using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCamera : MonoBehaviour
{
    public Transform cam;

    void Update()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
