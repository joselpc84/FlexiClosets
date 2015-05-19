using UnityEngine;
using System.Collections;

public class UiArrowActivator : MonoBehaviour
{
    public Item wallItem;
    public BotonGuiWall[] botons;

    public void ActivateBotons()
    {
        for (int i = 0; i < botons.Length; ++i)
        {
            botons[i].CheckIfEnable();
        }
        GUI_ItemController.Instance.item = wallItem;
        GUI_ItemController.Instance.WallItem.alpha = 1;
        GUI_ItemController.Instance.WallItem.blocksRaycasts = true;
        GUI_ItemController.Instance.WallItem.interactable = true;
    }

    public void DeActivateBotons()
    {
        for (int i = 0; i < botons.Length; ++i)
        {
            botons[i].gameObject.SetActive(false);
        }
        GUI_ItemController.Instance.WallItem.alpha = 0;
        GUI_ItemController.Instance.WallItem.blocksRaycasts = false;
        GUI_ItemController.Instance.WallItem.interactable = false;
    }
}
