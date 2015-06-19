using UnityEngine;
using System.Collections;

public class PopupItem : MonoBehaviour
{
    public CanvasGroup canvas;

    public void ShowPopUp()
    {
        // PopUp.gameObject.transform.localScale = new Vector3(1, 1, 1);
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }

    public void HidePopUp()
    {
        //PopUp.gameObject.transform.localScale = Vector3.zero;
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
    }

}
