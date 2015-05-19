﻿using UnityEngine;
using System.Collections;

public class GUI_ItemController : PersistentSingleton<GUI_ItemController>
{
    public Item item;
    //  public GameObject[] Buttons;
    public CanvasGroup groupCanvas;
    public Transform piso;
    public MouseOrbit CameraOrbit;

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
            item.Remove();
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
