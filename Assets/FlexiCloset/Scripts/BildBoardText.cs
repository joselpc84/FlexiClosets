using UnityEngine;
using System.Collections;

public class BildBoardText : MonoBehaviour
{

    Transform go;

    void Start()
    {
    
        go = Camera.main.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 heading = go.position - transform.position;

        transform.LookAt(transform.position - heading);
    }
}
