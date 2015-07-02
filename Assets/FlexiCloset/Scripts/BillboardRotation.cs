using UnityEngine;
using System.Collections;

public class BillboardRotation : MonoBehaviour
{

    public Transform center;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 heading = Camera.main.transform.position - transform.position;
        transform.LookAt(transform.position - heading);
        /*
        Vector3 from = (transform.position - Camera.main.transform.position).normalized;
        Vector3 to = (center.position - Camera.main.transform.position).normalized;

        Debug.DrawLine(transform.position, Camera.main.transform.position);
        Debug.DrawLine(center.position, Camera.main.transform.position);

      
       

        Vector3 localTarget = transform.InverseTransformPoint(to);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        transform.RotateAround(center.position, Vector3.up, -targetAngle); */

    }

}
