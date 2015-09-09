using UnityEngine;
using System.Collections;

public class BotonGuiItem : BotonGuiWall
{
	public bool RotateLeft = true;

	protected override bool OnClick ()
	{
		if (RotateLeft) {
			GUI_ItemController.Instance.RotaeLeft ();
		} else {
			GUI_ItemController.Instance.RotaeRigth ();

		}
		//	StopCoroutine ("ResetClick");
		//	StartCoroutine ("ResetClick", false);

		return true;
	}
}
