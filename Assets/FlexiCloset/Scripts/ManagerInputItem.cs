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

	// Update is called once per frame
	void LateUpdate ()
	{
		if (!isClickOnGUI) {

			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, ItemLayer)) {
				if (Input.GetMouseButtonDown (0)) {

					ResetCurrentSelected ();

					currentSelected = hitInfo.collider.GetComponent<Item> ();
					if (!currentSelected.OnClicked ()) {
						currentSelected = null;
					}
				}
			} else {
				if (Input.GetMouseButtonDown (0)) {
					ResetAll ();
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
		}
	}



	public void HardReset ()
	{
		if (currentSelected)
			currentSelected.OnCancel ();
		currentSelected = null;
	}
}
