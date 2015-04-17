using UnityEngine;
using System.Collections;

public class BotonGuiWall : MonoBehaviour
{
    public Collider _collider;
    public Wall wall;

    #region AuxVars:

    public LayerMask LayerGUI;

    RaycastHit hitInfo;
    Ray ray;

    #endregion

    bool isCliked = false;

    void OnEnable()
    {
        isCliked = false;
    }

    public void CheckIfEnable()
    {
        Vector3 direction = gameObject.transform.position - wall.transform.position;
        direction.y = 0;
        QuadInfo quad;
        ManagerGrid.getCenterNear(wall.transform.position + direction.normalized * ManagerGrid.Instance.Size, out quad);
        if (ManagerItemGrid.Instance.isEmptySpot(quad, ManagerItemGrid.Instance.wallPrefab))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerGUI))
            {
                if (hitInfo.collider == _collider)
                {
                    ManagerInputItem.Instance.isClickOnGUI = true;
                    isCliked = true;
                    OnClick();
                }
            }
        }
        /*    else if (Input.GetMouseButtonUp(0))
        {
            if (isCliked)
            {
                StopCoroutine("ResetClick");
                StartCoroutine("ResetClick", false);

            }
        }*/

    }

    IEnumerator ResetClick(bool value)
    {
    
        yield return new WaitForSeconds(0.1f);
        ManagerInputItem.Instance.isClickOnGUI = value;
        isCliked = value;
        gameObject.SetActive(false);

    }



    void OnClick()
    {
        Vector3 direction = gameObject.transform.position - wall.transform.position;
        direction.y = 0;
        QuadInfo quad;
        ManagerGrid.getCenterNear(wall.transform.position + direction.normalized * ManagerGrid.Instance.Size, out quad);
        if (ManagerItemGrid.Instance.isEmptySpot(quad, ManagerItemGrid.Instance.wallPrefab))
        {
            Item itemSpawned = ManagerItemGrid.Instance.wallPrefab.Spawn();
            itemSpawned.OnDrag();
            itemSpawned.OnDrop();
            ManagerItemGrid.Instance.AddItem(quad, itemSpawned);

            StopCoroutine("ResetClick");
            StartCoroutine("ResetClick", false);

        }
        else
        {
            Debug.LogError("No hay espacio");
        }
        
    }

    void OnFinish()
    {
    }
}