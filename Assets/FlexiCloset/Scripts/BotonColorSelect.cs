using UnityEngine;
using System.Collections;

public class BotonColorSelect : Boton3DUI
{
	public string ColorMat = "";

	protected override bool OnClick ()
	{
		EditorObjectPopUp.Instance.ChangueMaterial (ColorMat);

		return true;
	}
}
