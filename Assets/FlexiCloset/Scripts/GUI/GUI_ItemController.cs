using UnityEngine;
using System.Collections;

public class GUI_ItemController : PersistentSingleton<GUI_ItemController>
{
    public Item item;

    public void RotaeLeft()
    {
        if (item != null)
        {
            item.Rotate(-1);
        }
    }

    public void RotaeRigth()
    {
        if (item != null)
        {
            item.Rotate(1);
        }
    }


    public void ActivateGUI(Item item)
    {
        this.item = item;
        ManagerInputItem.Instance.isClickOnGUI = true;

    }

    public void DeActivateGUI()
    {
        this.item = null;
        ManagerInputItem.Instance.isClickOnGUI = false;

    }
}
