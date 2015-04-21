using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall : Item
{

    public UiArrowActivator GUI;

    public Wall leftWall;
    public Wall rightWall;
    public Wall upWall;
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

    public override void OnDrop()
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

}
