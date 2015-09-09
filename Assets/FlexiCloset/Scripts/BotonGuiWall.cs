using UnityEngine;
using System.Collections;

public class BotonGuiWall : MonoBehaviour
{
	public Collider _collider;
	public Wall wall;

	#region AuxVars:

	public LayerMask LayerGUI;

	RaycastHit hitInfo;
	Ray ray;

	#endregion

	public void CheckIfEnable ()
	{
		Vector3 direction = gameObject.transform.position - wall.transform.position;
		direction.y = 0;
		QuadInfo quad;
		ManagerGrid.getCenterNear (wall.transform.position + direction.normalized * ManagerGrid.Instance.Size, out quad);
		if (ManagerItemGrid.Instance.isEmptySpot (quad, ManagerItemGrid.Instance.wallPrefab)) {
			gameObject.SetActive (true);
		} else {
			gameObject.SetActive (false);
		}

	}

	public WallSpawnArrastre arrastre;

	protected virtual void Update ()
	{
		if (Input.GetMouseButton (0) && !ManagerInputItem.Instance.isClickOnGUI) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, LayerGUI)) {
				if (hitInfo.collider == _collider) {
					if (OnClick ()) {
						ManagerInputItem.Instance.SetIsClickOnGuiWithTimer (true);
						if (arrastre != null) {
						
							WallSpawnArrastre spa = arrastre.Spawn (gameObject.transform.position);
							spa.wall = wall;
						}
					}

				}
			}
		}

	}

	protected IEnumerator ResetClick (bool value)
	{
    
		yield return new WaitForSeconds (0.1f);
		ManagerInputItem.Instance.isClickOnGUI = value;
	}

	protected virtual bool OnClick ()
	{

		Vector3 direction = gameObject.transform.position - wall.transform.position;
		direction.y = 0;
		QuadInfo quad;
		ManagerGrid.getCenterNear (wall.transform.position + direction.normalized * ManagerGrid.Instance.Size, out quad);
		if (ManagerItemGrid.Instance.isEmptySpot (quad, ManagerItemGrid.Instance.wallPrefab)) {
			Item itemSpawned = ManagerItemGrid.Instance.wallPrefab.Spawn ();
			itemSpawned.OnDrag ();
			ManagerItemGrid.Instance.AddItem (quad, itemSpawned);
			itemSpawned.OnDrop ();
			ManagerInputItem.Instance.HardReset ();

			ManagerInputItem.Instance.SelectItem (itemSpawned);
			//((Wall)itemSpawned).ResetGUI ();
			//	StopCoroutine ("ResetClick");
			//	StartCoroutine ("ResetClick", false);
			return true;

		}
		return false;
	}

	void OnFinish ()
	{
	}
}