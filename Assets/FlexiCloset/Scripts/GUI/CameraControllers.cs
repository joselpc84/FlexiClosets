using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraControllers : MonoBehaviour
{
	public Scrollbar MaxController;
	public Text infoText;
	public Scrollbar.ScrollEvent Zoom;
	public string ZoomText;
	public Scrollbar.ScrollEvent Orbit;
	public string OrbitText;
	public Scrollbar.ScrollEvent Giro;
	public string GiroText;
	public Scrollbar.ScrollEvent PanX;
	public string PanXText;
	public Scrollbar.ScrollEvent PanY;
	public string PanYText;
	
	// Update is called once per frame
	void SetEvent (Scrollbar.ScrollEvent eventes, string tex)
	{
		MaxController.onValueChanged = eventes;
		infoText.text = tex;
	}

	public void SetZoom ()
	{
		SetEvent (Zoom, ZoomText);
	}

	public void SetOrbit ()
	{
		SetEvent (Orbit, OrbitText);
	}

	public void SetGiro ()
	{
		SetEvent (Giro, GiroText);
	}

	public void SetPanX ()
	{
		SetEvent (PanX, PanXText);
	}

	public void SetPanY ()
	{
		SetEvent (PanY, PanYText);
	}
}
