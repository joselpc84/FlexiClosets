using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour
{

    public Transform target;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    protected float distance = 25;
    public float minDistance = 10;
    public float MaxDistance = 25;

    public float Value
    {
        set
        {

            // 0  ----- mindistanece
            // value ---- 0.5f ---- (maxDist - Mindist)*value + mindistance
            // 1 -------- maxdistance

            float valueToMove = (MaxDistance - minDistance) * value + minDistance;
            distance = valueToMove;
        }
    }

    public float ValueX
    {
        set
        {
            float valueToMove = (360) * value;
            x = valueToMove;
            UpdateTransform();
        }
    }

    float x = 0.0f;
    float y = 0.0f;

    public string AxisX = "Mouse X";
    public string AxisY = "Mouse Y";

    public float InputX = 0;
    public float InputY = 0;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        distance = MaxDistance;
    }

    public void SetInputX(float value)
    {
        InputX = value;
    }

    public void SetInputY(float value)
    {
        InputY = value;
    }

    void LateUpdate()
    {
        if (target)
        {
            x += InputX * xSpeed * Time.deltaTime;
            y -= InputY * ySpeed * Time.deltaTime;

            UpdateTransform();

        }

    }

    protected void UpdateTransform()
    {
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;
        transform.position = rotation * (new Vector3(0, 0, -distance)) + target.position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }


}