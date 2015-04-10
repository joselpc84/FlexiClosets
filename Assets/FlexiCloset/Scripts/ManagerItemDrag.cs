using UnityEngine;
using System.Collections;

public class ManagerItemDrag : PersistentSingleton<ManagerItemDrag>
{

    protected Item itemSpawned = null;


    public void OnDrag(Item item)
    {
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

                ManagerItemGrid.Instance.AddWall(ManagerMouseControl.Instance.CurrentMousePos().Value, itemSpawned);

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
                    itemSpawned.transform.position = itemSpawned.transform.position + Vector3.up * 1f;
                    itemSpawned.ChangueColorPlane(Color.red);


                }
            }
        }
    }
}
