using UnityEngine;
using System.Collections;

public class GUI_ItemController : PersistentSingleton<GUI_ItemController>
{
	public Item item;

	public void RotaeLeft ()
	{
		if (item != null) {
			StopCoroutine ("ResetClick");
			StartCoroutine ("ResetClick", false);
			item.Rotate (-1);
		}
	}

	public void RotaeRigth ()
	{
		if (item != null) {
			StopCoroutine ("ResetClick");
			StartCoroutine ("ResetClick", false);
			item.Rotate (1);
		}
	}

	public void Move ()
	{
		if (item != null) {
			//StopCoroutine ("ResetClick");
			//StartCoroutine ("ResetClick", false);
			item.Move ();
		}
	}

	public void Remove ()
	{
		if (item != null) {
			StopCoroutine ("ResetClick");
			StartCoroutine ("ResetClick", false);
			item.Remove ();
		}
	}

	public void SetNoInput ()
	{
		StopCoroutine ("ResetClick");
		ManagerInputItem.Instance.isClickOnGUI = true;

	}

	IEnumerator ResetClick (bool value)
	{

		yield return new WaitForSeconds (0.2f);
		ManagerInputItem.Instance.isClickOnGUI = value;

	}

	public void ActivateGUI (Item item)
	{
		this.item = item;
	}

	public void DeActivateGUI ()
	{
		this.item = null;
	}
        

}
