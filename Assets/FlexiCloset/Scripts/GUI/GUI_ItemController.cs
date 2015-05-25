using UnityEngine;
using System.Collections;

public class GUI_ItemController : PersistentSingleton<GUI_ItemController>
{
    public Item item;
    //  public GameObject[] Buttons;
    public CanvasGroup groupCanvas;
    public Transform piso;
    public MouseOrbit CameraOrbit;

    public CanvasGroup[] WallItem;

    public void TurnOnWall()
    {
        for (int i = 0; i < WallItem.Length; ++i)
        {
            GUI_ItemController.Instance.WallItem[i].alpha = 1;
            GUI_ItemController.Instance.WallItem[i].blocksRaycasts = true;
            GUI_ItemController.Instance.WallItem[i].interactable = true;
        }

    }

    public void TurnOffWall()
    {
        for (int i = 0; i < WallItem.Length; ++i)
        {
            GUI_ItemController.Instance.WallItem[i].alpha = 0;
            GUI_ItemController.Instance.WallItem[i].blocksRaycasts = false;
            GUI_ItemController.Instance.WallItem[i].interactable = false;
        }

    }

    void Awake()
    {
        GUI_ItemController.Instance.DeActivateGUI();
    }

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

    public void Move()
    {
        if (item != null)
        {
            //StopCoroutine ("ResetClick");
            //StartCoroutine ("ResetClick", false);
            item.Move(true);
        }
    }

    public void Remove()
    {
        if (item != null)
        {
            if (CameraOrbit.target == item.transform)
            {
                CameraOrbit.target = piso;
            }

            item.Remove();
            BlockInput(false);
        }
    }

    public void RemoveAllWall()
    {
        if (item != null && item is Wall)
        {
            ((Wall)item).EraseAll = true;
            item.Remove();
            item = null;
            BlockInput(false);
        }
    }

    public void RemoveWall()
    {
        if (item != null && item is Wall)
        {
            item.Remove();
            item = null;
            BlockInput(false);
        }
    }

    public void CenterCamera()
    {
        if (item != null)
        {
            if (CameraOrbit.target != item.transform)
            {
                CameraOrbit.target = item.transform;
            }
            else
            {
                CameraOrbit.target = piso;
            }
        }
    }

    public void BlockInput(bool value)
    {
        ManagerInputItem.Instance.isClickOnGUI = value;

    }


    public void ActivateGUI(Item item)
    {
        this.item = item;
        /*
        for (int i = 0; i < Buttons.Length; ++i)
            Buttons[i].SetActive(true);
            */
        groupCanvas.alpha = 1;
        groupCanvas.interactable = true;
    }

    public void DeActivateGUI()
    {
        this.item = null;
        /*
        for (int i = 0; i < Buttons.Length; ++i)
            Buttons[i].SetActive(false);
*/
        groupCanvas.alpha = 0;
        groupCanvas.interactable = false;
    }
        

}
