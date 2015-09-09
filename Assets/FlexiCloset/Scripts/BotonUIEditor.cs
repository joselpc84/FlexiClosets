using UnityEngine;
using System.Collections;

public class BotonUIEditor : BotonGuiWall
{
	public Animator animEditor;
	public string openEditorAnim = "Open";

	bool opening = false;
	public float timeOpening = 0.25f;

	protected override bool OnClick ()
	{
		if (opening)
			return false;
		opening = true;
		animEditor.SetBool (openEditorAnim, !animEditor.GetBool (openEditorAnim));
		Invoke ("SetOpeningFalse", timeOpening);

		//	StopCoroutine ("ResetClick");
		//	StartCoroutine ("ResetClick", false);
		return true;
	}

	void SetOpeningFalse ()
	{
		opening = false;

	}

	void OnDisable ()
	{
		animEditor.SetBool (openEditorAnim, false);
		opening = false;

	}

}
