using UnityEngine;
using System.Collections;

/// <summary>
/// Maneja si lso items se pueden seleccionar o no.
/// </summary>
public class ManagerInputItem : PersistentSingleton<ManagerInputItem>
{

    public LayerMask ItemLayer;


    #region AuxVars:

    RaycastHit hitInfo;
    Ray ray;

    #endregion

    Collider helper;

    Item currentSelected;

    public bool isClickOnGUI = false;

    // Update is called once per frame
    void LateUpdate()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ItemLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isClickOnGUI)
                {
                    if (helper != hitInfo.collider)
                    {
                        helper = hitInfo.collider;
                        ResetCurrentSelected();
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {

                if (helper == hitInfo.collider)
                {
                    if (!isClickOnGUI)
                    {
                        currentSelected = hitInfo.collider.GetComponent<Item>();
                        if (!currentSelected.OnClicked())
                        {
                            currentSelected = null;
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                ResetAll();

            }
            else if (Input.GetMouseButtonUp(0))
            {
                ResetAll();

            }
        }
    }

    void ResetAll()
    {
        ResetHelper();
        ResetCurrentSelected();
    }

    void ResetCurrentSelected()
    {
        if (currentSelected && !isClickOnGUI)
        {
            currentSelected.OnCancel();

        }
    }

    void ResetHelper()
    {
        if (!isClickOnGUI)
        {
            helper = null;

        }
    }

    public void HardReset()
    {
        if (currentSelected)
            currentSelected.OnCancel();
        currentSelected = null;
        helper = null;
    }
}
