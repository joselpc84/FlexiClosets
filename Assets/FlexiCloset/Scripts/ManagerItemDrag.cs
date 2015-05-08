using UnityEngine;
using System.Collections;

/// <summary>
/// Manager item drag Maneja la logica desde el arrastrado hasta el drop
/// de los items
/// </summary>
public class ManagerItemDrag : PersistentSingleton<ManagerItemDrag>
{

    protected Item itemSpawned = null;
    public float OffSetY = 0.01f;
    bool NotSpawned = false;

    public void OnDrag(Item item)
    {
        InGameUI.Instance.OffPanel();

        ManagerInputItem.Instance.isClickOnGUI = true;
        NotSpawned = false;
        itemSpawned = item.Spawn();
        if (ManagerMouseControl.Instance.CurrentMousePos().HasValue)
        {
            itemSpawned.SetPos(ManagerMouseControl.Instance.CurrentMousePos().Value);
        }
        else
        {
            itemSpawned.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        itemSpawned.OnDrag();
    }

    public void OnDragWhitoutSpawn(Item item)
    {
        ManagerInputItem.Instance.isClickOnGUI = true;

        itemSpawned = item;
        StopCoroutine("WaitSetNotSpawned");
        StartCoroutine("WaitSetNotSpawned", true);
        if (ManagerMouseControl.Instance.CurrentMousePos().HasValue)
        {
            itemSpawned.SetPos(ManagerMouseControl.Instance.CurrentMousePos().Value);
        }
        else
        {
            itemSpawned.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        itemSpawned.OnDrag();
    }

    public void OnDrop()
    {
        if (ManagerMouseControl.Instance.CurrentMousePos().HasValue)
        {
            if (ManagerItemGrid.Instance.isEmptySpot(ManagerMouseControl.Instance.CurrentMousePos().Value, itemSpawned))
            {
                itemSpawned.OnDrop();

                ManagerItemGrid.Instance.AddItem(ManagerMouseControl.Instance.CurrentMousePos().Value, itemSpawned);

            }
            else
            {
                itemSpawned.Recycle();

            }
        }
        else
        {
            itemSpawned.Recycle();

        }
        NotSpawned = false;
        itemSpawned = null;
        StopCoroutine("ResetClick");
        StartCoroutine("ResetClick", false);

    }

    IEnumerator WaitSetNotSpawned(bool value)
    {

        yield return new WaitForSeconds(0.2f);
        NotSpawned = value;
    }

    IEnumerator ResetClick(bool value)
    {

        yield return new WaitForSeconds(0.2f);
        ManagerInputItem.Instance.isClickOnGUI = value;
    }

    void Update()
    {

        if (itemSpawned != null)
        {
            if (ManagerMouseControl.Instance.CurrentMousePos().HasValue)
            {
               
                itemSpawned.SetPos(ManagerMouseControl.Instance.CurrentMousePos().Value);
                if (ManagerItemGrid.Instance.isEmptySpot(ManagerMouseControl.Instance.CurrentMousePos().Value, itemSpawned))
                {
                    itemSpawned.ChangueColorPlane(Color.green);
                    //Aqui CAmbio las texturas etc, etc
                }
                else
                {
                    itemSpawned.transform.position = itemSpawned.transform.position + Vector3.up * OffSetY;
                    itemSpawned.ChangueColorPlane(Color.red);

                }

                if (NotSpawned && Input.GetMouseButtonDown(0))
                {
                    OnDrop();
                }
            }
        }
    }
}
