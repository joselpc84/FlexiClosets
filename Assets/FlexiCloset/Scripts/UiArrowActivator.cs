using UnityEngine;
using System.Collections;

public class UiArrowActivator : MonoBehaviour
{
	public Item wallItem;
	public BotonGuiWall[] botons;

	public virtual void ActivateBotons ()
	{
		StopCoroutine ("Hide");
		StartCoroutine ("Show");
	}

	public virtual void DeActivateBotons ()
	{
		StopCoroutine ("Show");
		StartCoroutine ("Hide");

	}

	protected virtual IEnumerator Show ()
	{
		yield return new WaitForSeconds (0.1f);
		for (int i = 0; i < botons.Length; ++i) {
			botons [i].CheckIfEnable ();
		}
		GUI_ItemController.Instance.item = wallItem;
		GUI_ItemController.Instance.TurnOnWall ();
	}

	protected virtual IEnumerator Hide ()
	{
		yield return new WaitForSeconds (0);
		for (int i = 0; i < botons.Length; ++i) {
			botons [i].gameObject.SetActive (false);
		}
		GUI_ItemController.Instance.TurnOffWall ();
	}
}
