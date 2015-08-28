using UnityEngine;
using System.Collections;

public class WallSpawnArrastre : MonoBehaviour
{
	public Wall wall;
	bool firtTime = true;
	Vector3 lastMousePosition;

	protected  void Update ()
	{
		if (firtTime) {
			direction = gameObject.transform.position - wall.transform.position;
			direction.y = 0;
			firtTime = false;
		}

		if (Input.GetMouseButton (0)) {
			OnClick ();
		} else {
			this.Recycle ();
		}
	}

	Vector3 direction;
	public float minDistance = 0.5f;
	public float maxangle = 20;

	void OnEnable ()
	{
		firtTime = true;
		Vector3? posMouse = ManagerMouseControl.Instance.CurrentMousePosNotQuad ();
		if (!posMouse.HasValue) {
			this.Recycle ();
			return;
		}

		lastMousePosition = posMouse.Value;

	}

	protected void OnClick ()
	{
		Vector3? posMouse = ManagerMouseControl.Instance.CurrentMousePosNotQuad ();
		if (!posMouse.HasValue) {
			this.Recycle ();
			return;
		}
		float distance = Vector3.Distance (lastMousePosition, posMouse.Value);
		Vector3 directionAux = posMouse.Value - lastMousePosition;
		float angle = Vector3.Angle (directionAux.normalized, direction);
		if (distance > minDistance && angle < maxangle && angle > -maxangle) {
		} else {
			return;
		}
		QuadInfo quad;
		ManagerGrid.getCenterNear (transform.position, out quad);
		if (ManagerItemGrid.Instance.isEmptySpot (quad, ManagerItemGrid.Instance.wallPrefab)) {
			Item itemSpawned = ManagerItemGrid.Instance.wallPrefab.Spawn ();
			itemSpawned.OnDrag ();
			ManagerItemGrid.Instance.AddItem (quad, itemSpawned);
			itemSpawned.OnDrop ();
			ManagerInputItem.Instance.HardReset ();

			ManagerInputItem.Instance.SelectItem (itemSpawned);
			((Wall)itemSpawned).ResetGUI ();
			transform.position =	itemSpawned.PositionStart.center + direction.normalized * ManagerGrid.Instance.Size;
		
			lastMousePosition = itemSpawned.transform.position;

		} else {
			this.Recycle ();
		}

	}
}
