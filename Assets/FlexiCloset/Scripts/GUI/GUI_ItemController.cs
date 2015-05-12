using UnityEngine;
using System.Collections;

public class GUI_ItemController : PersistentSingleton<GUI_ItemController>
{
	public Item item;

	public void RotaeLeft ()
	{
		if (item != null) {
			item.Rotate (-1);
		}
	}

	public void RotaeRigth ()
	{
		if (item != null) {
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
			item.Remove ();
		}
	}

	public void BlockInput (bool value)
	{
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
