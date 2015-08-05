using UnityEngine;
using System.Collections;

public class UiArrowActivator_Item : UiArrowActivator
{
	public MeshRenderer[] renderes;
	/*
	public override void ActivateBotons ()
	{


	}

	public override void DeActivateBotons ()
	{


	}*/

	protected override IEnumerator Show ()
	{
		yield return new WaitForSeconds (0.25f);
		for (int i = 0; i < botons.Length; ++i) {
			botons [i].enabled = true;
			renderes [i].enabled = true;
		}

	}

	protected override IEnumerator Hide ()
	{
		yield return new WaitForSeconds (0);
		for (int i = 0; i < botons.Length; ++i) {

			botons [i].enabled = false;
			renderes [i].enabled = false;
		}
	}
}
