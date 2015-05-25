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
        GUI_ItemController.Instance.TurnOnWall();

    }

    public void DeActivateBotons()
    {
        for (int i = 0; i < botons.Length; ++i)
        {
            botons[i].gameObject.SetActive(false);
        }
        GUI_ItemController.Instance.TurnOffWall();

    }
}
