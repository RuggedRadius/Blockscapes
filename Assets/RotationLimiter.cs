using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLimiter : MonoBehaviour
{
    public float xMinValue;
    public float xMaxValue;
    public float yMinValue;
    public float yMaxValue;
    public float zMinValue;
    public float zMaxValue;

    void Update()
    {
        // Limit X
        if (transform.eulerAngles.x < xMinValue)
            transform.eulerAngles = new Vector3(xMinValue, transform.eulerAngles.y, transform.eulerAngles.z);
        if (transform.eulerAngles.x > xMaxValue)
            transform.eulerAngles = new Vector3(xMaxValue, transform.eulerAngles.y, transform.eulerAngles.z);

        // Limit Y
        if (transform.eulerAngles.y < yMinValue)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yMinValue, transform.eulerAngles.z);
        if (transform.eulerAngles.y > yMaxValue)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yMaxValue, transform.eulerAngles.z);

        // Limit Z
        if (transform.eulerAngles.z < zMinValue)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zMinValue);
        if (transform.eulerAngles.z > zMaxValue)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zMaxValue);
    }
}
