using UnityEngine;
using System.Collections;

/// <summary>
/// Maneja si lso items se pueden seleccionar o no.
/// </summary>
public class ManagerInputItem : PersistentSingleton<ManagerInputItem>
{

	public LayerMask ItemLayer;


	#region AuxVars:

	RaycastHit hitInfo;
	Ray ray;

	#endregion

	Item currentSelected;

	public bool isClickOnGUI = false;

	Vector3 MouseDown;

	// Update is called once per frame
	void LateUpdate ()
	{
		if (!isClickOnGUI) {

			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, ItemLayer)) {
				if (Input.GetMouseButtonDown (0)) {

					ResetAll ();

					currentSelected = hitInfo.collider.GetComponent<Item> ();
					if (!currentSelected.OnClicked ()) {
						currentSelected = null;
					} else {
						MouseDown = Input.mousePosition;
					}
				} 
			} else {
				if (Input.GetMouseButtonDown (0)) {
					ResetAll ();
				}
			}

			if (Input.GetMouseButton (0)) {
				if (currentSelected != null) {
					if (Vector3.Distance (MouseDown, Input.mousePosition) > 14) {
						Item aux = currentSelected;
						ResetAll ();
						aux.Move (false);
					}

				}
			}
		}

	}

	void ResetAll ()
	{
		ResetCurrentSelected ();
	}

	public void ResetCurrentSelected ()
	{
		if (currentSelected) {
			currentSelected.OnCancel ();
			currentSelected = null;

		}
	}

	public void HardReset ()
	{
		if (currentSelected)
			currentSelected.OnCancel ();
		currentSelected = null;
	}
}
