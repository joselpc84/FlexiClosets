using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall : Item
{

    public UiArrowActivator GUI;

    //   [HideInInspector]
    public Wall leftWall;
    //  [HideInInspector]
    public Wall rightWall;
    //  [HideInInspector]
    public Wall upWall;
    //   [HideInInspector]
    public Wall downWall;

    [HideInInspector]
    public bool EraseAll = false;

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
        EraseAll = false;
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
        if (vecino != null && vecino != this)
        {
            leftWall = vecino;
            vecino.rightWall = this;
        }

        ManagerGrid.getCenterNear(transform.position + Vector3.right * ManagerGrid.Instance.Size, out _quad);
        vecino = ManagerItemGrid.Instance.getWallIfInSpot(_quad, this);
        if (vecino != null && vecino != this)
        {
            rightWall = vecino;
            vecino.leftWall = this;
        }

        ManagerGrid.getCenterNear(transform.position + Vector3.forward * ManagerGrid.Instance.Size, out _quad);
        vecino = ManagerItemGrid.Instance.getWallIfInSpot(_quad, this);
        if (vecino != null && vecino != this)
        {
            upWall = vecino;
            vecino.downWall = this;
        }

        ManagerGrid.getCenterNear(transform.position - Vector3.forward * ManagerGrid.Instance.Size, out _quad);
        vecino = ManagerItemGrid.Instance.getWallIfInSpot(_quad, this);
        if (vecino != null && vecino != this)
        {
            downWall = vecino;
            vecino.upWall = this;
        }
    }

    protected override void LogicOnClicked()
    {
        GUI.ActivateBotons();
    }

    public override void Remove()
    {


        if (EraseAll)
        {
            if (leftWall != null)
                leftWall.RemoveVeci(0);

            if (rightWall != null)
                rightWall.RemoveVeci(1);
        
            if (upWall != null)
                upWall.RemoveVeci(2);
        
            if (downWall != null)
                downWall.RemoveVeci(3);
        }
        ManagerInputItem.Instance.HardReset();
        ManagerItemGrid.Instance.RemoveItem(this);
        this.Recycle();
    }

    public  void RemoveVeci(int dir)
    {


        switch (dir)
        {
            case 0:
                if (leftWall != null)
                    leftWall.RemoveVeci(dir);
                break;
            case 1:
                if (rightWall != null)
                    rightWall.RemoveVeci(dir);
                break;
            case 2:
                if (upWall != null)
                    upWall.RemoveVeci(dir);
                break;
            case 3:
                if (downWall != null)
                    downWall.RemoveVeci(dir);
                break;
        }

        ManagerInputItem.Instance.HardReset();
        ManagerItemGrid.Instance.RemoveItem(this);       
        this.Recycle();
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
