using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum TypeControllerCamera
{
	Zoom,
	Orbit,
	Giro,
	PanX,
	PanY
}

public class CameraControllers : MonoBehaviour
{
	public MouseOrbit cameraController;

	public Scrollbar MaxController;
	public Text infoText;
	TypeControllerCamera currentSelected = TypeControllerCamera.Zoom;

	public Scrollbar.ScrollEvent Zoom;
	public string ZoomText;
	public float ZoomStoreValue = 0;

	public Scrollbar.ScrollEvent Orbit;
	public string OrbitText;
	public float OrbitStoreValue = 0;

	public Scrollbar.ScrollEvent Giro;
	public string GiroText;
	public float GiroStoreValue = 0;

	public Scrollbar.ScrollEvent PanX;
	public string PanXText;
	public float PanXStoreValue = 0;

	public Scrollbar.ScrollEvent PanY;
	public string PanYText;
	public float PanYStoreValue = 0;

	void Start ()
	{
	
		ResetValues ();
	}

	public void ResetValues ()
	{
		ZoomStoreValue = cameraController.InitialZoom;
		Zoom.Invoke (ZoomStoreValue);

		OrbitStoreValue = cameraController.InitialPosY;
		Orbit.Invoke (OrbitStoreValue);

		GiroStoreValue = cameraController.InitialPosX;
		Giro.Invoke (GiroStoreValue);

		PanXStoreValue = 0.5f;
		PanX.Invoke (PanXStoreValue);

		PanYStoreValue = 0.5f;
		PanY.Invoke (PanYStoreValue);


		MaxController.onValueChanged = Zoom;
		MaxController.value = ZoomStoreValue;
		currentSelected = TypeControllerCamera.Zoom;
		infoText.text = ZoomText;
	}
	// Update is called once per frame
	void SetEvent (TypeControllerCamera newSelect, Scrollbar.ScrollEvent eventes, string tex, float valueStore)
	{
		switch (currentSelected) {
		case TypeControllerCamera.Zoom:
			ZoomStoreValue = MaxController.value;
			if (newSelect == currentSelected)
				valueStore = ZoomStoreValue;
			break;
		case TypeControllerCamera.Orbit:
			OrbitStoreValue = MaxController.value;
			if (newSelect == currentSelected)
				valueStore = OrbitStoreValue;
			break;
		case TypeControllerCamera.Giro:
			GiroStoreValue = MaxController.value;
			if (newSelect == currentSelected)
				valueStore = GiroStoreValue;
			break;
		case TypeControllerCamera.PanX:
			PanXStoreValue = MaxController.value;
			if (newSelect == currentSelected)
				valueStore = PanXStoreValue;
			break;
		case TypeControllerCamera.PanY:
			PanYStoreValue = MaxController.value;
			if (newSelect == currentSelected)
				valueStore = PanYStoreValue;
			break;
		}
	
		
		MaxController.onValueChanged = eventes;
		infoText.text = tex;
		MaxController.value = valueStore;
		currentSelected = newSelect;
	}

	public void SetZoom ()
	{
		SetEvent (TypeControllerCamera.Zoom, Zoom, ZoomText, ZoomStoreValue);
	}

	public void SetZoomValue (float value)
	{
		ZoomStoreValue = value;
		if (TypeControllerCamera.Zoom == currentSelected)
			MaxController.value = ZoomStoreValue;
		else {
			Zoom.Invoke (ZoomStoreValue);
		}
	}

	public void SetOrbit ()
	{
		SetEvent (TypeControllerCamera.Orbit, Orbit, OrbitText, OrbitStoreValue);
	}

	public void SetOrbitValue (float value)
	{
		OrbitStoreValue = value;
		if (TypeControllerCamera.Orbit == currentSelected)
			MaxController.value = OrbitStoreValue;
		else {
			Orbit.Invoke (OrbitStoreValue);
		}
	}

	public void SetGiro ()
	{
		SetEvent (TypeControllerCamera.Giro, Giro, GiroText, GiroStoreValue);
	}

	public void SetGiroValue (float value)
	{
		GiroStoreValue = value;
		if (TypeControllerCamera.Giro == currentSelected)
			MaxController.value = GiroStoreValue;
		else {
			Giro.Invoke (GiroStoreValue);
		}
	}

	public void SetPanX ()
	{
		SetEvent (TypeControllerCamera.PanX, PanX, PanXText, PanXStoreValue);
	}

	public void SetPanXValue (float value)
	{
		PanXStoreValue = value;
		if (TypeControllerCamera.PanX == currentSelected)
			MaxController.value = PanXStoreValue;
		else {
			PanX.Invoke (PanXStoreValue);
		}
	}

	public void SetPanY ()
	{
		SetEvent (TypeControllerCamera.PanY, PanY, PanYText, PanYStoreValue);
	}

	public void SetPanYValue (float value)
	{
		PanYStoreValue = value;
		if (TypeControllerCamera.PanY == currentSelected)
			MaxController.value = PanYStoreValue;
		else {
			PanY.Invoke (PanYStoreValue);
		}
	}

	bool _isShow = true;

	public bool isShow {

		get {

			return _isShow;
		}
		set {
			_isShow = !_isShow;

		}
	}

	public void SetIsShow (bool value)
	{
		_isShow = value;

	}

	public void LateUpdate ()
	{
		if (isShow)
			MaxController.value += Input.GetAxis ("Mouse ScrollWheel");
	}
}
