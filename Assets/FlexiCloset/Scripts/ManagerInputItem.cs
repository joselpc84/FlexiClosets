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
	bool _isClickOnGUI = false;

	public bool isClickOnGUI {
		get {
			return _isClickOnGUI;
		}
		set {
			_isClickOnGUI = value;
			CancelInvoke ("ResetClick");
		}
	}

	public void SetIsClickOnGuiWithTimer (bool value)
	{
		_isClickOnGUI = value;
		if (_isClickOnGUI) {
			CancelInvoke ("ResetClick");
			Invoke ("ResetClick", 0.1f);
		}
	}

	protected void ResetClick ()
	{
		_isClickOnGUI = false;
	}

	Vector3 MouseDown;

	// Update is called once per frame
	void LateUpdate ()
	{
		if (!isClickOnGUI) {

			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, ItemLayer)) {
				if (Input.GetMouseButtonDown (0)) {
					SelectItem (hitInfo.collider.GetComponent<Item> ());
				} 

		
			} else {
				if (Input.GetMouseButtonDown (0)) {
					ResetAll ();
				}
			}

			if (Input.GetMouseButton (0)) {
				if (currentSelected != null && !(currentSelected is Wall)) {
					if (Vector3.Distance (MouseDown, Input.mousePosition) > 5
					    && Vector3.Distance (MouseDown, Input.mousePosition) < 15
					    && !GUI_ItemController.Instance.waitInput) {
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
		HardReset ();
	}

	public void HardReset ()
	{
		if (currentSelected)
			currentSelected.OnCancel ();
		currentSelected = null;
	}

	public void SelectItem (Item item)
	{
		ResetAll ();

		currentSelected = item;
		if (currentSelected == null) {
			return;
		}
		if (!currentSelected.OnClicked ()) {
			currentSelected = null;
		} else {
			MouseDown = Input.mousePosition;
		}

	}
}
