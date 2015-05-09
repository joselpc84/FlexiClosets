using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DirectionFace
{

    NegZ = 0,
    NegX = 1,
    PosZ = 2,
    PosX = 3
}

public class Item : MonoBehaviour
{

    public int SizeBack = 1;
    public int SizeRight = 1;
    public MeshRenderer[] mesh;
    public MeshRenderer[] planes;
    protected DirectionFace DirectionForward = DirectionFace.NegZ;

    protected QuadInfo PositionStart;

	public float High = 0.1f;

	public int getStartPosition {
		get {
			return PositionStart.index;
		}
	}

	protected List<int> brothers = new List<int> ();

	public List<int> getBrothers {
		get {
			return brothers;
		}
	}

	protected bool isDropped = false;

    protected Collider _coll;

    public Collider _collider
    {
    
        get
        {
            if (_coll == null)
            {
                _coll = GetComponent<Collider>();

            }
            return _coll;
        }
    }

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

    public void Remove()
    {
        ManagerItemGrid.Instance.RemoveItem(this);
        OnCancel();
        this.Recycle();
    }

    public void Rotate(int dir)
    {
        DirectionFace storeDirectionForward = DirectionForward;
        Quaternion storeRotation = transform.rotation;

        ManagerItemGrid.Instance.RemoveItem(this);

        DirectionForward = DirectionForward - dir;
        if ((int)DirectionForward >= 4)
            DirectionForward = DirectionFace.NegZ;
        else if ((int)DirectionForward < 0)
            DirectionForward = DirectionFace.PosX;

        transform.Rotate(Vector3.up, 90 * dir);

        if (!ManagerItemGrid.Instance.isEmptySpot(PositionStart, this))
        {
            DirectionForward = storeDirectionForward;
            transform.rotation = storeRotation;
        }

        ManagerItemGrid.Instance.AddItem(PositionStart, this);


    }

    public void Move()
    {
        ManagerItemGrid.Instance.RemoveItem(this);
        OnCancel();
        ManagerItemDrag.Instance.OnDragWhitoutSpawn(this);
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
        
        SelectedMesh();


    }

    #region Visual MEsh

    protected void SelectedMesh()
    {
        for (int i = 0; i < mesh.Length; ++i)
            LeanTween.color(mesh[i].gameObject, new Color(0.5f, 0.5f, 0.5f, 0.5f), 1.0f).setLoopPingPong(); 

        for (int i = 0; i < planes.Length; ++i)
        {
            planes[i].gameObject.SetActive(true);
        }
    }

    protected void DeSelectedMesh()
    {
        for (int i = 0; i < mesh.Length; ++i)
            LeanTween.cancel(mesh[i].gameObject);

        for (int i = 0; i < mesh.Length; ++i)
            foreach (Material mat in mesh[i].materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.color = new Color(1, 1, 1, 1);
                }
                else if (mat.HasProperty("_TintColor"))
                {
//                    Color col = mat.GetColor("_TintColor");
                    mat.SetColor("_TintColor", new Color(1, 1, 1, 1));
                }

            }


        for (int i = 0; i < planes.Length; ++i)
        {
            planes[i].gameObject.SetActive(false);
        }
    }

    #endregion

    public virtual void OnDrop()
    {
        

        DeSelectedMesh();


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
        SelectedMesh();
        isSelected = true;
        LogicOnClicked();
        return true;

    }

    protected virtual void LogicOnClicked()
    {
        GUI_ItemController.Instance.ActivateGUI(this);

    }

    public void OnCancel()
    {
        if (!isDropped && !isSelected)
            return;
        DeSelectedMesh();
        isSelected = false;

        LogicOnCancel();
    }

    protected virtual void LogicOnCancel()
    {
        GUI_ItemController.Instance.DeActivateGUI();

    }

    #endregion
}
