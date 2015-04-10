﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DirectionFace
{

    PosX,
    NegX,
    PosZ,
    NegZ
}

public class Item : MonoBehaviour
{

    public int SizeBack = 1;
    public int SizeRight = 1;
    public MeshRenderer[] mesh;
    public MeshRenderer[] planes;
    protected  DirectionFace DirectionForward = DirectionFace.NegZ;

    protected QuadInfo PositionStart;

    protected List<int> brothers = new List<int>();


    void OnEnable()
    {
        brothers.Clear();
        PositionStart.index = -1;
        PositionStart.center = Vector3.zero;
        DirectionForward = DirectionFace.NegZ;
    }

    public void Rotate(DirectionFace toDirection)
    {
        //   brothers.Clear();
    }

    void SetBrother(DirectionFace face, int size)
    {
        int pastIndex = PositionStart.index;
        for (int i = 0; i < size - 1; ++i)
        {
            int newBro = -1;
            switch (face)
            {
                case DirectionFace.PosZ:
                    newBro = ManagerGrid.Instance.getIndexUp(pastIndex);
                    break;
                case DirectionFace.PosX:
                    newBro = ManagerGrid.Instance.getIndexRight(pastIndex);
                    break;
                case DirectionFace.NegZ:
                    newBro = ManagerGrid.Instance.getIndexDown(pastIndex);
                    break;
                case DirectionFace.NegX:
                    newBro = ManagerGrid.Instance.getIndexLeft(pastIndex);
                    break;
            }

            if (newBro != -1)
            {
                brothers.Add(newBro);
                pastIndex = newBro;
            }
        }
    }

    public List<int> getPosibleBrothers(int index)
    {
        brothers.Clear();
        //Derecha: NegX
        SetBrother(DirectionFace.NegX, SizeRight);
        //Atras: PosZ
        SetBrother(DirectionFace.PosZ, SizeBack);
        List<int> posibleList = new List<int>(brothers);
        brothers.Clear();
        return posibleList;
    }


    public void SetQuad(QuadInfo quad)
    {
        brothers.Clear();
        PositionStart.center = quad.center;
        PositionStart.index = quad.index;

        transform.position = PositionStart.center;

        //PosZ = Up
        //NegZ = Down
        //PosX = Rigth
        //NegX = Left
        switch (DirectionForward)
        {
            case DirectionFace.NegX:
                //Derecha: PosZ
                SetBrother(DirectionFace.PosZ, SizeRight);
                //Atras: PosX
                SetBrother(DirectionFace.PosX, SizeBack);
                break;
            case DirectionFace.NegZ:
                //Derecha: NegX
                SetBrother(DirectionFace.NegX, SizeRight);
                //Atras: PosZ
                SetBrother(DirectionFace.PosZ, SizeBack);
                break;
            case DirectionFace.PosX:
                //Derecha: NegZ
                SetBrother(DirectionFace.NegZ, SizeRight);
                //Atras: NegX
                SetBrother(DirectionFace.NegX, SizeBack);
                break;
            case DirectionFace.PosZ:
                //Derecha: PosX
                SetBrother(DirectionFace.PosX, SizeRight);
                //Atras: NegZ
                SetBrother(DirectionFace.NegZ, SizeBack);
                break;
        }


    }

    public int SpotID
    {
    
        get
        {
        
            return PositionStart.index;
        }
    }

    public int[] SpotBrothersID
    {

        get
        {

            return brothers.ToArray();
        }
    }

    #region BuildStage:

    public void OnDrag()
    {
        for (int i = 0; i < mesh.Length; ++i)
            LeanTween.alpha(mesh[i].gameObject, 0.2f, 1.0f).setLoopPingPong(); 

        for (int i = 0; i < planes.Length; ++i)
        {
            planes[i].gameObject.SetActive(true);
        }
    }

    public void OnDrop()
    {
        for (int i = 0; i < mesh.Length; ++i)
            LeanTween.cancel(mesh[i].gameObject);

        for (int i = 0; i < mesh.Length; ++i)
            foreach (Material mat in mesh[i].materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
                }
                else if (mat.HasProperty("_TintColor"))
                {
                    Color col = mat.GetColor("_TintColor");
                    mat.SetColor("_TintColor", new Color(col.r, col.g, col.b, 1));
                }

            }
        for (int i = 0; i < planes.Length; ++i)
        {
            planes[i].gameObject.SetActive(false);
        }
    }

    public void ChangueColorPlane(Color color)
    {
        for (int i = 0; i < planes.Length; ++i)
        {
            foreach (Material mat in planes[i].materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = color;
                }
                else if (mat.HasProperty("_TintColor"))
                {
                    Color col = color;
                    mat.SetColor("_TintColor", new Color(col.r, col.g, col.b, 1));
                }

            }
        }
    }

    #endregion

}
