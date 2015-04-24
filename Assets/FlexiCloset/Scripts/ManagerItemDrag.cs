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

    public void OnDrag(Item item)
    {
        ManagerInputItem.Instance.isClickOnGUI = true;

        itemSpawned = item.Spawn();
        if (ManagerMouseControl.Instance.CurrentMousePos().HasValue)
        {
            itemSpawned.SetQuad(ManagerMouseControl.Instance.CurrentMousePos().Value);
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

        itemSpawned = null;
        StopCoroutine("ResetClick");
        StartCoroutine("ResetClick", false);

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
               
                itemSpawned.SetQuad(ManagerMouseControl.Instance.CurrentMousePos().Value);
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
            }
        }
    }
}
