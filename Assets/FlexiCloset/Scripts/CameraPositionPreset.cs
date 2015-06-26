using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

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

    [Serializable]
    public class PresetPostion
    {
    
        public Vector3 Position;
        public Vector3 Rotation;
        public float Zoom;
        public float Orbit;
        public float RotationValue;
    }

    public PresetPostion posA;
    public PresetPostion posB;
    public PresetPostion posC;
    public PresetPostion posD;
    PresetPostion currentPreset;
    public float TimeToReach = 1.0f;
    LTDescr pos;
    LTDescr rot;

    public void GoToLeft()
    {

        //C = 0,0.25 
        //B = 0.75, 1
        //A= 0.5,0.75
        //D= 0.25,0.5
        if ((0 <= OrbitController_Y.value) && (OrbitController_Y.value < posC.RotationValue))
        {
            currentPreset = posC;
        }
        else if ((posC.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.25f))
        {
            currentPreset = posD;
        }
        else if ((0.25f <= OrbitController_Y.value) && (OrbitController_Y.value < posD.RotationValue))
        {
            currentPreset = posD;
        }
        else if ((posD.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.5f))
        {
            currentPreset = posA;
        }
        else if ((0.5f <= OrbitController_Y.value) && (OrbitController_Y.value < posA.RotationValue))
        {
            currentPreset = posA;
        }
        else if ((posA.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.75f))
        {
            currentPreset = posB;
        }
        else if ((0.75f <= OrbitController_Y.value) && (OrbitController_Y.value < posB.RotationValue))
        {
            currentPreset = posB;
        }
        else if ((posB.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value <= 1.0f))
        {
            Debug.Log("Este Giro da problema de B a C");

            currentPreset = posC;
        }

        controller.enabled = false;
        if (pos != null)
            pos.pause();
        if (rot != null)
            rot.pause();
        pos = LeanTween.value(gameObject, OnPosChanged, transform.position, currentPreset.Position, TimeToReach);
        pos = pos.setEase(LeanTweenType.linear);
        pos.onComplete = OnCompelteAll;
        rot = LeanTween.value(gameObject, OnRotChanged, transform.rotation.eulerAngles, currentPreset.Rotation, TimeToReach);
        rot = rot.setEase(LeanTweenType.linear);

    }

    public void GoToRight()
    {
        if ((0 <= OrbitController_Y.value) && (OrbitController_Y.value < posC.RotationValue))
        {
            currentPreset = posB;
        }
        else if ((posC.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.25f))
        {
            currentPreset = posB;
        }
        else if ((0.25f <= OrbitController_Y.value) && (OrbitController_Y.value < posD.RotationValue))
        {
            currentPreset = posC;
        }
        else if ((posD.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.5f))
        {
            currentPreset = posC;
        }
        else if ((0.5f <= OrbitController_Y.value) && (OrbitController_Y.value < posA.RotationValue))
        {
            currentPreset = posD;
        }
        else if ((posA.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.75f))
        {
            currentPreset = posD;
        }
        else if ((0.75f <= OrbitController_Y.value) && (OrbitController_Y.value < posB.RotationValue))
        {
            currentPreset = posA;
        }
        else if ((posB.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value <= 1.0f))
        {

            currentPreset = posA;
        }

        controller.enabled = false;
        if (pos != null)
            pos.pause();
        if (rot != null)
            rot.pause();
        pos = LeanTween.value(gameObject, OnPosChanged, transform.position, currentPreset.Position, TimeToReach);
        pos = pos.setEase(LeanTweenType.linear);
        pos.onComplete = OnCompelteAll;
        rot = LeanTween.value(gameObject, OnRotChanged, transform.rotation.eulerAngles, currentPreset.Rotation, TimeToReach);
        rot = rot.setEase(LeanTweenType.linear);

    }

    void OnCompelteAll()
    {
        controller.enabled = true;

        ZoomController.value = currentPreset.Zoom;   
        OrbitController_X.value = currentPreset.Orbit;   
        OrbitController_Y.value = currentPreset.RotationValue;
    }

    void OnPosChanged(Vector3 pos)
    {
        transform.position = pos;
              
    }

    void OnRotChanged(Vector3 rot)
    {
        transform.rotation = Quaternion.Euler(rot);

    }
        
}
