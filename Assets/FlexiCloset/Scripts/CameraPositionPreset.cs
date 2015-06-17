using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(MouseOrbit))]
public class CameraPositionPreset : MonoBehaviour
{
    MouseOrbit _controller;

    public MouseOrbit controller
    {
        get
        {
        
            if (object.ReferenceEquals(_controller, null))
                _controller = GetComponent<MouseOrbit>();
            return _controller;
                
        }
    }

    public Scrollbar ZoomController;
    public Scrollbar OrbitController_X;
    public Scrollbar OrbitController_Y;

    public float PosXLeft;
    public float PosYLeft;
    public float PosXRigth;
    public float PosYRigth;

    public void ShowLeft()
    {
        //Izqueirda -- 1
        // Derecha -- 0
        OrbitController_X.value = PosXLeft;
        OrbitController_Y.value = PosYLeft;
    }

    public void ShowRigth()
    {
        //Izqueirda -- 1
        // Derecha -- 0
        OrbitController_X.value = PosXRigth;
        OrbitController_Y.value = PosYRigth;
    }
}
