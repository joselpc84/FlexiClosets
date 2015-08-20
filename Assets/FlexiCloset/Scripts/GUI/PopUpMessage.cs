using UnityEngine;
using System.Collections;

public class PopUpMessage : MonoBehaviour
{
	
	public string Message;

	public void ShowError ()
	{
		ManagerPopUp.Instance.ShowError (GetInstanceID (), Message);
	}

	public void ShowTip ()
	{
		ManagerPopUp.Instance.ShowTip (GetInstanceID (), Message);
	}

	public void Stop ()
	{
		ManagerPopUp.Instance.Stop (GetInstanceID ());
	}
}
