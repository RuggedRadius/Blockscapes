using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float speed;

    void Update()
    {
        this.transform.localEulerAngles += Vector3.up * Time.deltaTime * speed;
    }
}
