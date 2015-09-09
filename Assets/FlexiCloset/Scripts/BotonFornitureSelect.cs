using UnityEngine;
using System.Collections;

public class BotonFornitureSelect : Boton3DUI
{
	public bool RotateLeft = true;
	public EditorForniture editorF;

	protected override bool OnClick ()
	{
		if (RotateLeft) {
			editorF.LeftMesh ();
		} else {
			editorF.RigthMesh ();
		}
		return true;
	}
}
