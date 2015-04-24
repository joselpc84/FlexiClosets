using UnityEngine;
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
    public  DirectionFace DirectionForward = DirectionFace.NegZ;

    protected QuadInfo PositionStart;

    protected List<int> brothers = new List<int>();

    protected bool isDropped = false;

    public int SpotID
    {

        get
        {

            return PositionStart.index;
        }
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        brothers.Clear();
        PositionStart.index = -1;
        PositionStart.center = Vector3.zero;
        DirectionForward = DirectionFace.NegZ;

        isDropped = false;
    }

    public void Rotate(DirectionFace toDirection)
    {
        brothers.Clear();
    }



    #region BrothersLogic:

    int[] SetBrother(int pastIndex, DirectionFace face, int size)
    {
        int[] brotherAux = new int[size - 1];
        for (int i = 0; i < size - 1; ++i)
        {
            int newBro = -1;
            switch (face)
            {
                case DirectionFace.PosZ:
                    newBro = ManagerGrid.Instance.getIndexDown(pastIndex);
                    break;
                case DirectionFace.PosX:
                    newBro = ManagerGrid.Instance.getIndexRight(pastIndex);
                    break;
                case DirectionFace.NegZ:
                    newBro = ManagerGrid.Instance.getIndexUp(pastIndex);
                    break;
                case DirectionFace.NegX:
                    newBro = ManagerGrid.Instance.getIndexLeft(pastIndex);
                    break;
            }

            if (newBro != -1)
            {
                brothers.Add(newBro);
                brotherAux[i] = newBro;
                pastIndex = newBro;
            }
        }
        return brotherAux;
    }

    public List<int> getPosibleBrothers(int index)
    {

        SetBrothersList(index);

        List<int> posibleList = new List<int>(brothers);
        brothers.Clear();
        return posibleList;
    }

    protected void SetBrothersList(int index)
    {
        brothers.Clear();
        //PosZ = Up
        //NegZ = Down
        //PosX = Rigth
        //NegX = Left
        int[] listRightBro;
        switch (DirectionForward)
        {
            case DirectionFace.NegX:
                //Derecha: PosZ
                listRightBro = SetBrother(index, DirectionFace.PosZ, SizeRight);
                //Atras: PosX
                SetBrother(index, DirectionFace.PosX, SizeBack);
                for (int i = 0; i < listRightBro.Length; ++i)
                    SetBrother(listRightBro[i], DirectionFace.PosX, SizeBack);
                break;
            case DirectionFace.NegZ:
                //Derecha: NegX
                listRightBro = SetBrother(index, DirectionFace.NegX, SizeRight);
                //Atras: PosZ
                SetBrother(index, DirectionFace.PosZ, SizeBack);
                for (int i = 0; i < listRightBro.Length; ++i)
                    SetBrother(listRightBro[i], DirectionFace.PosZ, SizeBack);
                break;
            case DirectionFace.PosX:
                //Derecha: NegZ
                listRightBro = SetBrother(index, DirectionFace.NegZ, SizeRight);
                //Atras: NegX
                SetBrother(index, DirectionFace.NegX, SizeBack);
                for (int i = 0; i < listRightBro.Length; ++i)
                    SetBrother(listRightBro[i], DirectionFace.NegX, SizeBack);
                break;
            case DirectionFace.PosZ:
                //Derecha: PosX
                listRightBro = SetBrother(index, DirectionFace.PosX, SizeRight);
                //Atras: NegZ
                SetBrother(index, DirectionFace.NegZ, SizeBack);
                for (int i = 0; i < listRightBro.Length; ++i)
                    SetBrother(listRightBro[i], DirectionFace.NegZ, SizeBack);
                break;
        }
    }

    public virtual void SetQuad(QuadInfo quad)
    {
        PositionStart.center = quad.center;
        PositionStart.index = quad.index;

        transform.position = PositionStart.center;

        SetBrothersList(PositionStart.index);
    }

    public virtual void SetPos(QuadInfo quad)
    {
        PositionStart.center = quad.center;
        PositionStart.index = quad.index;
        transform.position = PositionStart.center;

    }

    public int[] SpotBrothersID
    {

        get
        {

            return brothers.ToArray();
        }
    }

    #endregion

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

    public virtual void OnDrop()
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

        //New Stuff
        StartCoroutine(SetIsDropped());
    }

    IEnumerator SetIsDropped()
    {
    
        yield return new WaitForSeconds(0.3f);
        isDropped = true;

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

    #region ModifiedStage:

    bool isSelected = false;

    public bool OnClicked()
    {
    
        if (!isDropped && isSelected)
            return false;

        isSelected = true;
        LogicOnClicked();
        return true;

    }

    protected virtual void LogicOnClicked()
    {
    }

    public void OnCancel()
    {
        if (!isDropped && !isSelected)
            return;

        isSelected = false;

        LogicOnCancel();
    }

    protected virtual void LogicOnCancel()
    {
    }

    #endregion
}
