using UnityEngine;
using System.Collections;

/// <summary>
/// Manager item drag Maneja la logica desde el arrastrado hasta el drop
/// de los items
/// </summary>
public class ManagerItemDrag : PersistentSingleton<ManagerItemDrag>
{

	protected Item itemSpawned = null;
	public float OffSetY = 0.01f;
	bool NotSpawned = false;
	bool useDropDown = false;

	bool canDrop = false;

	public void OnSpawn (Item item, int posGrid)
	{
		QuadInfo? infoQ = ManagerMouseControl.Instance.CurrentMousePos (posGrid);
		#region OnDrag/Spawn Logic:
		if (itemSpawned != null) {
			itemSpawned.Recycle ();
			NotSpawned = false;
			itemSpawned = null;
		}
		InGameUI.Instance.OffPanel ();//Hago que se enconda el panel
		NotSpawned = false;
		itemSpawned = item.Spawn (item.transform.position, item.transform.rotation);

		itemSpawned.SetPos (infoQ.Value);
		this.useDropDown = false;
		NotSpawned = true;
		canDrop = false;
		itemSpawned.OnDrag ();
		#endregion
		#region OnDrop:
		if (ManagerItemGrid.Instance.isEmptySpot (infoQ.Value, itemSpawned)) {
			itemSpawned.SetPos (infoQ.Value);
			itemSpawned.OnDrop ();
			ManagerItemGrid.Instance.AddItem (infoQ.Value, itemSpawned);
		} else {
			itemSpawned.Recycle ();
		}
		NotSpawned = false;
		itemSpawned = null;
		#endregion
	}

	public void OnDrag (Item item)
	{
		if (itemSpawned != null) {
			itemSpawned.Recycle ();
			NotSpawned = false;
			itemSpawned = null;
		}

		InGameUI.Instance.OffPanel ();//Hago que se enconda el panel

		ManagerInputItem.Instance.isClickOnGUI = true;
		NotSpawned = false;
		itemSpawned = item.Spawn (item.transform.position, item.transform.rotation);
		if (ManagerMouseControl.Instance.CurrentMousePos ().HasValue) {
			itemSpawned.SetPos (ManagerMouseControl.Instance.CurrentMousePos ().Value);
		} else {
			itemSpawned.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}
		this.useDropDown = false;//NuevaForma para dar click

		NotSpawned = true;
		canDrop = false;
		StartCoroutine ("ResetDrop");
		//
		itemSpawned.OnDrag ();
	}

	public void OnDragWhitoutSpawn (Item item, bool useDropDown)
	{
		if (itemSpawned != null) {
			itemSpawned.Recycle ();
			NotSpawned = false;
			itemSpawned = null;
		}

		ManagerInputItem.Instance.isClickOnGUI = true;
		this.useDropDown = useDropDown;
		itemSpawned = item;
		NotSpawned = true;
		canDrop = false;
		StartCoroutine ("ResetDrop");
		itemSpawned.OnDrag ();
	}

	IEnumerator ResetDrop ()
	{
		yield return new WaitForSeconds (0.2f);
		canDrop = true;

	}

	public void OnDrop ()
	{
		Item test;

		if (ManagerMouseControl.Instance.CurrentMousePos ().HasValue) {
			if (ManagerItemGrid.Instance.isEmptySpot (ManagerMouseControl.Instance.CurrentMousePos ().Value, itemSpawned)) {
				itemSpawned.SetPos (ManagerMouseControl.Instance.CurrentMousePos ().Value);
				itemSpawned.OnDrop ();
				ManagerItemGrid.Instance.AddItem (ManagerMouseControl.Instance.CurrentMousePos ().Value, itemSpawned);

			} else if ((test = ManagerItemGrid.Instance.CanPutUp (ManagerMouseControl.Instance.CurrentMousePos ().Value, itemSpawned)) != null) {
				while (test.DirectionForward != itemSpawned.DirectionForward) {
					itemSpawned.RotatePreview (1);
				}
				itemSpawned.SetPos (test.PositionStart);
				itemSpawned.OnDrop (true);
				ManagerItemGrid.Instance.AddItem (test.PositionStart, itemSpawned, true);
				itemSpawned.transform.position = itemSpawned.transform.position + Vector3.up * test.getMaxHeighPut ();
				test.AddItemUp (itemSpawned);


			} else {
				itemSpawned.Recycle ();

			}

		} else {
			itemSpawned.Recycle ();

		}
		NotSpawned = false;
		itemSpawned = null;
		StopCoroutine ("ResetClick");
		StartCoroutine ("ResetClick", false);

	}

	IEnumerator WaitSetNotSpawned (bool value)
	{

		yield return new WaitForSeconds (0.2f);
		NotSpawned = value;
	}

	IEnumerator ResetClick (bool value)
	{
		yield return new WaitForSeconds (0.2f);
		ManagerInputItem.Instance.isClickOnGUI = value;
	}

	void Update ()
	{
		if (itemSpawned != null) {
			if (ManagerMouseControl.Instance.CurrentMousePos ().HasValue) {
				Item test;
				itemSpawned.SetPos (ManagerMouseControl.Instance.CurrentMousePos ().Value);
				if (ManagerItemGrid.Instance.isEmptySpot (ManagerMouseControl.Instance.CurrentMousePos ().Value, itemSpawned)) {
					itemSpawned.ChangueColorPlane (Color.green);
					//Aqui CAmbio las texturas etc, etc
				} else if ((test = ManagerItemGrid.Instance.CanPutUp (ManagerMouseControl.Instance.CurrentMousePos ().Value, itemSpawned)) != null) {
					itemSpawned.transform.position = test.PositionStart.center;
					itemSpawned.transform.position = itemSpawned.transform.position + Vector3.up * test.getMaxHeighPut ();
					itemSpawned.ChangueColorPlane (Color.green);

					while (test.DirectionForward != itemSpawned.DirectionForward) {
						itemSpawned.RotatePreview (1);
					}


				} else {
					itemSpawned.transform.position = itemSpawned.transform.position;//+ Vector3.up * OffSetY;
					itemSpawned.ChangueColorPlane (Color.red);

				}

				if (NotSpawned && canDrop) {
					if (useDropDown) {
						if (Input.GetMouseButtonDown (0)) {
							OnDrop ();

						}	
					} else {
						if (Input.GetMouseButtonUp (0)) {
							OnDrop ();
						}
					}
				}
			}
		}
	}
}
