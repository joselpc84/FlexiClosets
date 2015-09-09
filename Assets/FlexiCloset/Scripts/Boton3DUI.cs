using UnityEngine;
using System.Collections;

public class Boton3DUI : MonoBehaviour
{
	public Collider _collider;

	#region AuxVars:

	public LayerMask LayerGUI;

	RaycastHit hitInfo;
	Ray ray;

	#endregion

	protected virtual void Update ()
	{
		if (Input.GetMouseButton (0) && !ManagerInputItem.Instance.isClickOnGUI) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, LayerGUI)) {
				if (hitInfo.collider == _collider) {
					if (OnClick ()) {
						ManagerInputItem.Instance.SetIsClickOnGuiWithTimer (true);
					}

				}
			}
		}

	}

	protected virtual bool OnClick ()
	{
		return false;
	}
}
