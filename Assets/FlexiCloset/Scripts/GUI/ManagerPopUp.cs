using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManagerPopUp : PersistentSingleton<ManagerPopUp>
{


	public Animator anim;
	public Text title;
	public Text error;
	public float DefaultTimeTipOff = 5.0f;
	public float DefaultTimeErrorOff = 2.0f;
	protected int currentID = -1;

	public void ShowError (int ID, string texto)
	{
		StopCoroutine ("RequestOff");

		title.text = "Error";
		if (currentID != ID) {
			error.text = "";
			anim.SetTrigger ("In");
		}
		error.text = texto;
		StartCoroutine ("RequestOff", DefaultTimeErrorOff);
		currentID = ID;
		
	}

	public void ShowTip (int ID, string texto)
	{
		StopCoroutine ("RequestOff");
		
		title.text = "Tip";
		if (currentID != ID) {
			error.text = "";
			anim.SetTrigger ("In");
		}
		error.text = texto;
		StartCoroutine ("RequestOff", DefaultTimeTipOff);
		currentID = ID;


	}

	public void Off ()
	{
		anim.SetTrigger ("Out");
		currentID = -1;
	}

	public void Stop (int ID)
	{
		if (ID == currentID) {
			StopCoroutine ("RequestOff");
			Off ();
		}
	}

	IEnumerator RequestOff (float time)
	{
	
		yield return new WaitForSeconds (time);
		Off ();
	}

}
