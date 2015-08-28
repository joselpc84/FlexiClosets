using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent (typeof(MouseOrbit))]
public class CameraPositionPreset : MonoBehaviour
{
	MouseOrbit _controller;

	public MouseOrbit controller {
		get {
        
			if (object.ReferenceEquals (_controller, null))
				_controller = GetComponent<MouseOrbit> ();
			return _controller;
                
		}
	}

	public Scrollbar ZoomController;
	public Scrollbar OrbitController_X;
	public Scrollbar OrbitController_Y;
	public Scrollbar PanController_X;
	public Scrollbar PanController_Y;

	public Button CameraControls;
	public ChangeCanvasButton canvasCameraControls;
	public ChangeColorButton colorCameraControls;

	public float PosXLeft;
	public float PosYLeft;
	public float PosXRigth;
	public float PosYRigth;

	public void ShowLeft ()
	{
		//Izqueirda -- 1
		// Derecha -- 0
		OrbitController_X.value = PosXLeft;
		OrbitController_Y.value = PosYLeft;
	}

	public void ShowRigth ()
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

	bool isReaching = false;

	public void GoToLeft ()
	{
		if (isReaching)
			return;

		controller.target = GUI_ItemController.Instance.piso;
		ResetPan ();

		//C = 0,0.25 
		//B = 0.75, 1
		//A= 0.5,0.75
		//D= 0.25,0.5
		if ((0 <= OrbitController_Y.value) && (OrbitController_Y.value < posC.RotationValue)) {
			currentPreset = posC;
		} else if ((posC.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.25f)) {
			currentPreset = posD;
		} else if ((0.25f <= OrbitController_Y.value) && (OrbitController_Y.value < posD.RotationValue)) {
			currentPreset = posD;
		} else if ((posD.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.5f)) {
			currentPreset = posA;
		} else if ((0.5f <= OrbitController_Y.value) && (OrbitController_Y.value < posA.RotationValue)) {
			currentPreset = posA;
		} else if ((posA.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.75f)) {
			currentPreset = posB;
		} else if ((0.75f <= OrbitController_Y.value) && (OrbitController_Y.value < posB.RotationValue)) {
			currentPreset = posB;
		} else if ((posB.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value <= 1.0f)) {
			currentPreset = posC;
		}

		controller.enabled = false;
		if (pos != null)
			pos.pause ();
		if (rot != null)
			rot.pause ();
		pos = LeanTween.value (gameObject, OnPosChanged, transform.position, currentPreset.Position, TimeToReach);
		pos.onComplete = OnCompelteAll;
		isReaching = true;
		// rot = LeanTween.value(gameObject, OnRotChanged, transform.rotation.eulerAngles, currentPreset.Rotation, TimeToReach);
		// rot = rot.setEase(LeanTweenType.linear);

	}

	public void GoToRight ()
	{
		if (isReaching)
			return;


		controller.target = GUI_ItemController.Instance.piso;
		isReaching = true;
		ResetPan ();

		if ((0 <= OrbitController_Y.value) && (OrbitController_Y.value < posC.RotationValue)) {
			currentPreset = posB;
		} else if ((posC.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.25f)) {
			currentPreset = posB;
		} else if ((0.25f <= OrbitController_Y.value) && (OrbitController_Y.value < posD.RotationValue)) {
			currentPreset = posC;
		} else if ((posD.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.5f)) {
			currentPreset = posC;
		} else if ((0.5f <= OrbitController_Y.value) && (OrbitController_Y.value < posA.RotationValue)) {
			currentPreset = posD;
		} else if ((posA.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value < 0.75f)) {
			currentPreset = posD;
		} else if ((0.75f <= OrbitController_Y.value) && (OrbitController_Y.value < posB.RotationValue)) {
			currentPreset = posA;
		} else if ((posB.RotationValue <= OrbitController_Y.value) && (OrbitController_Y.value <= 1.0f)) {

			currentPreset = posA;
		}

		controller.enabled = false;
		if (pos != null)
			pos.pause ();
		if (rot != null)
			rot.pause ();
		pos = LeanTween.value (gameObject, OnPosChanged, transform.position, currentPreset.Position, TimeToReach);
		pos.onComplete = OnCompelteAll;
		//    rot = LeanTween.value(gameObject, OnRotChanged, transform.rotation.eulerAngles, currentPreset.Rotation, TimeToReach);
		//    rot = rot.setEase(LeanTweenType.linear);

	}

	void OnCompelteAll ()
	{
		isReaching = false;
		controller.enabled = true;

		ZoomController.value = currentPreset.Zoom;   
		OrbitController_X.value = currentPreset.Orbit;   
		OrbitController_Y.value = currentPreset.RotationValue;
	}

	void OnPosChanged (Vector3 pos)
	{
		transform.position = pos;
		Camera.main.transform.LookAt (controller.target);         
	}

	void OnRotChanged (Vector3 rot)
	{
		transform.rotation = Quaternion.Euler (rot);

	}

	// A 0,6,100  - 0,180,0
	// B 100,6,0 - 0,270,0
	// C 0,6,-100 - 0,0,0
	// D -100,6,0 - 0,90,0

	float storeZoom = 0;
	float storeOrbitY = 0;
	float storeOrbitX = 0;
	public PopUpMessage tipOrto;

	IEnumerator BlockInputAlways ()
	{
	
		while (Camera.main.orthographic) {
			ManagerInputItem.Instance.isClickOnGUI = true;
			yield return null;
		}
	}

	public void GoToLeftOrto ()
	{
		if (isReaching && !Camera.main.orthographic)
			return;
        
		Camera.main.orthographic = !Camera.main.orthographic;
		controller.enabled = !Camera.main.orthographic;
		if (Camera.main.orthographic) {
			
			ResetPan ();

			ManagerInputItem.Instance.isClickOnGUI = true;
			ManagerInputItem.Instance.HardReset ();
			StopCoroutine ("BlockInputAlways");
			StartCoroutine ("BlockInputAlways");
			//ShowTip
			tipOrto.ShowTip ();

			CameraControls.interactable = false;
			canvasCameraControls.SetIsShow (false);
			colorCameraControls.SetIsShow (false);
			isReaching = true;
			Camera.main.orthographicSize = 22;
			Quaternion quat = Quaternion.Euler (0, 180, 0);
			Vector3 pos = new Vector3 (Camera.main.transform.position.x, 6, Camera.main.transform.position.z);

			if ((Camera.main.transform.position.x <= 10 && -10 <= Camera.main.transform.position.x) && (Camera.main.transform.position.z >= 10)) {
				pos = new Vector3 (0, 6, 100);
				quat = Quaternion.Euler (0, 180, 0);//1
			} else if ((Camera.main.transform.position.x >= 10) && (Camera.main.transform.position.z <= 10 && -10 <= Camera.main.transform.position.z)) {
				pos = new Vector3 (100, 6, 0);
				quat = Quaternion.Euler (0, 270, 0);//2
			} else if ((Camera.main.transform.position.x <= 10 && -10 <= Camera.main.transform.position.x) && (Camera.main.transform.position.z <= -10)) {
				pos = new Vector3 (0, 6, -100);
				quat = Quaternion.Euler (0, 0, 0);//3
			} else if ((Camera.main.transform.position.x <= -10) && (Camera.main.transform.position.z <= 10 && -10 <= Camera.main.transform.position.z)) {
				pos = new Vector3 (-100, 6, 0);
				quat = Quaternion.Euler (0, 90, 0);//4
			} else if ((Camera.main.transform.position.x >= 10) && (Camera.main.transform.position.z >= 10)) {
				pos = new Vector3 (100, 6, 0);
				quat = Quaternion.Euler (0, 270, 0);//2
			} else if ((Camera.main.transform.position.x >= 10) && (Camera.main.transform.position.z <= -10)) {
				pos = new Vector3 (0, 6, -100);
				quat = Quaternion.Euler (0, 0, 0);//3
			} else if ((Camera.main.transform.position.x <= -10) && (Camera.main.transform.position.z >= 10)) {
				pos = new Vector3 (0, 6, 100);
				quat = Quaternion.Euler (0, 180, 0);//1
			} else if ((Camera.main.transform.position.x <= -10) && (Camera.main.transform.position.z <= -10)) {
				pos = new Vector3 (-100, 6, 0);
				quat = Quaternion.Euler (0, 90, 0);//4
			} else {
				Debug.Log ("Otro Caso");
			}

			Camera.main.transform.rotation = quat;
			Camera.main.transform.position = pos;

			storeZoom = ZoomController.value;
			storeOrbitY = OrbitController_Y.value;
			storeOrbitX = OrbitController_X.value;
		} else {
			ResetPan ();

			ZoomController.value = storeZoom;   
			OrbitController_X.value = storeOrbitX;   
			OrbitController_Y.value = storeOrbitY;
			isReaching = false;
			CameraControls.interactable = true;

			tipOrto.Stop ();
			ManagerInputItem.Instance.isClickOnGUI = false;
			StopCoroutine ("BlockInputAlways");
		}
	}

	public void GoToRigthOrto ()
	{
		if (isReaching && !Camera.main.orthographic)
			return;

		Camera.main.orthographic = !Camera.main.orthographic;
		controller.enabled = !Camera.main.orthographic;
		if (Camera.main.orthographic) {

			ResetPan ();

			ManagerInputItem.Instance.isClickOnGUI = true;
			ManagerInputItem.Instance.HardReset ();
			StopCoroutine ("BlockInputAlways");
			StartCoroutine ("BlockInputAlways");
			//ShowTip
			tipOrto.ShowTip ();

			CameraControls.interactable = false;
			canvasCameraControls.SetIsShow (false);
			colorCameraControls.SetIsShow (false);
			isReaching = true;
			Camera.main.orthographicSize = 22;
			Quaternion quat = Quaternion.Euler (0, 180, 0);
			Vector3 pos = new Vector3 (Camera.main.transform.position.x, 6, Camera.main.transform.position.z);

			if ((Camera.main.transform.position.x <= 10 && -10 <= Camera.main.transform.position.x) && (Camera.main.transform.position.z >= 10)) {
				pos = new Vector3 (0, 6, 100);
				quat = Quaternion.Euler (0, 180, 0);//1
			} else if ((Camera.main.transform.position.x >= 10) && (Camera.main.transform.position.z <= 10 && -10 <= Camera.main.transform.position.z)) {
				pos = new Vector3 (100, 6, 0);
				quat = Quaternion.Euler (0, 270, 0);//2
			} else if ((Camera.main.transform.position.x <= 10 && -10 <= Camera.main.transform.position.x) && (Camera.main.transform.position.z <= -10)) {
				pos = new Vector3 (0, 6, -100);
				quat = Quaternion.Euler (0, 0, 0);//3
			} else if ((Camera.main.transform.position.x <= -10) && (Camera.main.transform.position.z <= 10 && -10 <= Camera.main.transform.position.z)) {
				pos = new Vector3 (-100, 6, 0);
				quat = Quaternion.Euler (0, 90, 0);//4
			} else if ((Camera.main.transform.position.x >= 10) && (Camera.main.transform.position.z >= 10)) {
				pos = new Vector3 (100, 6, 0);
				quat = Quaternion.Euler (0, 270, 0);//2
			} else if ((Camera.main.transform.position.x >= 10) && (Camera.main.transform.position.z <= -10)) {
				pos = new Vector3 (0, 6, -100);
				quat = Quaternion.Euler (0, 0, 0);//3
			} else if ((Camera.main.transform.position.x <= -10) && (Camera.main.transform.position.z >= 10)) {
				pos = new Vector3 (0, 6, 100);
				quat = Quaternion.Euler (0, 180, 0);//1
			} else if ((Camera.main.transform.position.x <= -10) && (Camera.main.transform.position.z <= -10)) {
				pos = new Vector3 (-100, 6, 0);
				quat = Quaternion.Euler (0, 90, 0);//4
			} else {
				Debug.Log ("Otro Caso");
			}

			Camera.main.transform.rotation = quat;
			Camera.main.transform.position = pos;

			storeZoom = ZoomController.value;
			storeOrbitY = OrbitController_Y.value;
			storeOrbitX = OrbitController_X.value;
		} else {
			ResetPan ();

			ZoomController.value = storeZoom;   
			OrbitController_X.value = storeOrbitX;   
			OrbitController_Y.value = storeOrbitY;
			isReaching = false;
			CameraControls.interactable = true;

			tipOrto.Stop ();
			ManagerInputItem.Instance.isClickOnGUI = false;
			StopCoroutine ("BlockInputAlways");
		}
	}

	public void CenterCamera ()
	{
		if (!isReaching) {
			CameraControls.interactable = true;
			isReaching = false;
			Camera.main.orthographic = false;
			controller.enabled = true;
			controller.target = GUI_ItemController.Instance.piso;
			controller.ResetPos ();
			ZoomController.value = controller.InitialZoom;   
			OrbitController_X.value = controller.InitialPosY;   
			OrbitController_Y.value = controller.InitialPosX;

			ResetPan ();

		}
	}

	public void ResetPan ()
	{
		PanController_X.value = 0.5f;
		PanController_Y.value = 0.5f;
	}
}
