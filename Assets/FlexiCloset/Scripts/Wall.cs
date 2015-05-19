using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall : Item
{

    public UiArrowActivator GUI;

    [HideInInspector]
    public Wall leftWall;
    [HideInInspector]
    public Wall rightWall;
    [HideInInspector]
    public Wall upWall;
    [HideInInspector]
    public Wall downWall;

    protected override void OnDisable()
    {
        base.OnDisable();
        //Aqui ver si broadcasteo a quien eliminar

        if (leftWall)
            leftWall.rightWall = null;

        if (rightWall)
            rightWall.leftWall = null;
        
        if (upWall)
            upWall.downWall = null;
        
        if (downWall)
            downWall.upWall = null;
        
        leftWall = null;
        rightWall = null;
        upWall = null;
        downWall = null;
    }

    public override void OnDrop(bool isUp = false)
    {
        base.OnDrop();
        AddWallVecinos();
    }

    protected void AddWallVecinos()
    {
        QuadInfo _quad;
        ManagerGrid.getCenterNear(transform.position + Vector3.left * ManagerGrid.Instance.Size, out _quad);
        Wall vecino = ManagerItemGrid.Instance.getWallIfInSpot(_quad, this);
        if (vecino != null)
        {
            leftWall = vecino;
            vecino.rightWall = this;
        }

        ManagerGrid.getCenterNear(transform.position + Vector3.right * ManagerGrid.Instance.Size, out _quad);
        vecino = ManagerItemGrid.Instance.getWallIfInSpot(_quad, this);
        if (vecino != null)
        {
            rightWall = vecino;
            vecino.leftWall = this;
        }

        ManagerGrid.getCenterNear(transform.position + Vector3.forward * ManagerGrid.Instance.Size, out _quad);
        vecino = ManagerItemGrid.Instance.getWallIfInSpot(_quad, this);
        if (vecino != null)
        {
            upWall = vecino;
            vecino.downWall = this;
        }

        ManagerGrid.getCenterNear(transform.position - Vector3.forward * ManagerGrid.Instance.Size, out _quad);
        vecino = ManagerItemGrid.Instance.getWallIfInSpot(_quad, this);
        if (vecino != null)
        {
            downWall = vecino;
            vecino.upWall = this;
        }
    }

    protected override void LogicOnClicked()
    {
        GUI.ActivateBotons();
    }



    protected override void LogicOnCancel()
    {
        GUI.DeActivateBotons();

    }

    IEnumerator ResetClick(bool value)
    {

        yield return new WaitForSeconds(0.1f);
        ManagerInputItem.Instance.isClickOnGUI = value;
        //   gameObject.SetActive(false);
    }

    public void ResetGUI()
    {
        StopCoroutine("ResetClick");
        StartCoroutine("ResetClick", false);
    }

}
