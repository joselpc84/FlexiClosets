using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerItemGrid : PersistentSingleton<ManagerItemGrid>
{

    Dictionary<int,Item> items = new Dictionary<int,Item>();

    public Wall wallPrefab;

    public void AddItem(QuadInfo info, Item item)
    {
        item.SetQuad(info);
        items.Add(item.SpotID, item);

        int[] bros = item.SpotBrothersID;
        for (int i = 0; i < bros.Length; ++i)
        {
            items.Add(bros[i], item);

        }
    }

    public bool isEmptySpot(QuadInfo quad, Item info)
    {
        bool isEmpty = true;
        List<int> posiblePosition = info.getPosibleBrothers(quad.index);
        foreach (var spot in items)
        {
            if (spot.Key == quad.index)
            {
                isEmpty = false;
            }
            else
            {
                for (int i = 0; i < posiblePosition.Count; ++i)
                {

                    if (spot.Key == posiblePosition[i])
                    {
                        isEmpty = false;
                    }
                }
            }
            if (!isEmpty)
                break;
        }
        return isEmpty;
    }
      
}
