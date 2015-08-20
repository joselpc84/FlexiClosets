using UnityEngine;
using System.Collections;

public class GUI_ItemController : PersistentSingleton<GUI_ItemController>
{
	public CanvasGroup PopUpCanvas;
	public CanvasGroup PopUpEditorCanvas;

	public Item item;
	//  public GameObject[] Buttons;
	public CanvasGroup groupCanvas;
	public Transform piso;
	public MouseOrbit CameraOrbit;

	public CanvasGroup[] WallItem;

	[HideInInspector]
	public bool waitInput = false;

	IEnumerator WaitForInput ()
	{
		yield return new WaitForSeconds (0.3f);
		waitInput = false;
	}

	void Awake ()
	{
		GUI_ItemController.Instance.DeActivateGUI ();
	}

	public void RotaeLeft ()
	{
		if (item != null && !waitInput) {
			item.Rotate (1);
			waitInput = true;
			StartCoroutine ("WaitForInput");
		}
	}

	public void RotaeRigth ()
	{
		if (item != null && !waitInput) {
			item.Rotate (-1);
			waitInput = true;
			StartCoroutine ("WaitForInput");
		}
	}

	public void Move ()
	{
		if (item != null && !waitInput) {
			item.Move (true);
			waitInput = true;
			StartCoroutine ("WaitForInput");
		}
	}

	public void Remove ()
	{
		if (item != null) {
			if (CameraOrbit.target == item.transform) {
				CameraOrbit.target = piso;
			}

			item.Remove ();
			BlockInput (false);

		}
	}

	public void RemoveAllWall ()
	{
		if (item != null && item is Wall) {
			((Wall)item).EraseAll = true;
			item.Remove ();
			item = null;
			BlockInput (false);
			HidePopObject ();
		}
	}

	public void RemoveWall ()
	{
		if (item != null && item is Wall) {
			item.Remove ();
			item = null;
			BlockInput (false);
			HidePopObject ();
		}
	}

	public PopUpMessage popupCenter;

	public void CenterCamera ()
	{
		if (!waitInput) {
			if (item != null) {
				if (CameraOrbit.target != item.transform) {
					CameraOrbit.target = item.transform;
					popupCenter.ShowTip ();
				}
			} else {
				CameraOrbit.target = piso;
			}
		}
	}

	public void BlockInput (bool value)
	{
		ManagerInputItem.Instance.isClickOnGUI = value;

	}


	public void ActivateGUI (Item item)
	{
		ShowPopObject ();
		this.item = item;

		groupCanvas.alpha = 1;
		groupCanvas.interactable = true;
		groupCanvas.blocksRaycasts = true;

	}

	public void DeActivateGUI ()
	{
		HidePopObject ();
		/*
        this.item = null;
        groupCanvas.alpha = 0;
        groupCanvas.interactable = false;
        groupCanvas.blocksRaycasts = false;*/

	}

	public void TurnOnWall ()
	{
		ShowPopObject ();
		for (int i = 0; i < WallItem.Length; ++i) {
			GUI_ItemController.Instance.WallItem [i].alpha = 1;
			GUI_ItemController.Instance.WallItem [i].blocksRaycasts = true;
			GUI_ItemController.Instance.WallItem [i].interactable = true;
		}

	}

	public void TurnOffWall ()
	{
		HidePopObject ();
		/*  for (int i = 0; i < WallItem.Length; ++i)
        {
            GUI_ItemController.Instance.WallItem[i].alpha = 0;
            GUI_ItemController.Instance.WallItem[i].blocksRaycasts = false;
            GUI_ItemController.Instance.WallItem[i].interactable = false;
        }*/
	}

	public void ShowPopEditor ()
	{
		PopUpEditorCanvas.alpha = 1;
		PopUpEditorCanvas.interactable = true;
		PopUpEditorCanvas.blocksRaycasts = true;
	}

	public void HidePopEditor ()
	{
		PopUpEditorCanvas.alpha = 0;
		PopUpEditorCanvas.interactable = false;
		PopUpEditorCanvas.blocksRaycasts = false;
	}

	public void ShowPopObject ()
	{
		PopUpCanvas.alpha = 1;
		PopUpCanvas.interactable = true;
		PopUpCanvas.blocksRaycasts = true;
		ModuloUI.Instance.HidePopUp ();
	}

	public void HidePopObject ()
	{
		HidePopEditor ();

		PopUpCanvas.alpha = 0;
		PopUpCanvas.interactable = false;
		PopUpCanvas.blocksRaycasts = false;

		for (int i = 0; i < WallItem.Length; ++i) {
			GUI_ItemController.Instance.WallItem [i].alpha = 0;
			GUI_ItemController.Instance.WallItem [i].blocksRaycasts = false;
			GUI_ItemController.Instance.WallItem [i].interactable = false;
		}

		this.item = null;
		groupCanvas.alpha = 0;
		groupCanvas.interactable = false;
		groupCanvas.blocksRaycasts = false;
	}

	public void TurnEditor ()
	{
		if (PopUpEditorCanvas.alpha > 0) {
			HidePopEditor ();
		} else {
			ShowPopEditor ();
		}
	}
}
