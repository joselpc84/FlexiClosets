using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Guarda lso items colocados en la escena
/// </summary>
public class ManagerItemGrid : PersistentSingleton<ManagerItemGrid>
{

    Dictionary<int,Item> items = new Dictionary<int,Item>();
    Dictionary<int,Wall> itemsWall = new Dictionary<int,Wall>();

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

        if (item is Wall)
        {
            itemsWall.Add(item.SpotID, item as Wall);

            for (int i = 0; i < bros.Length; ++i)
            {
                itemsWall.Add(bros[i], item as Wall);

            }
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item.SpotID);
        int[] bros = item.SpotBrothersID;
        for (int i = 0; i < bros.Length; ++i)
        {
            items.Remove(bros[i]);

        }

        if (item is Wall)
        {
            itemsWall.Remove(item.SpotID);
            for (int i = 0; i < bros.Length; ++i)
            {
                itemsWall.Remove(bros[i]);

            }

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

    public Wall getWallIfInSpot(QuadInfo quad, Wall info)
    {
    
        if (!isEmptySpot(quad, info))
        {
            if (items[quad.index] is Wall)
                return items[quad.index] as Wall;
        }
        return null;

    }

    public List< List<int>> getDistance()
    {
        List< List<int>> muros = new List< List<int>>();
        List<int> firstRow = new List<int>();

        foreach (var wall in itemsWall)
        {
            bool isInAnyOne = false;
            while (!isInAnyOne)
            {
                foreach (List<int> listi in muros)
                {
                    if (listi.Contains(wall.Value.SpotID))
                    {
                        isInAnyOne = true;
                    }
                }
                break;
            }

            if (!isInAnyOne)
            {
                
                firstRow.Add(wall.Value.SpotID);
                Wall right = wall.Value.rightWall;
                while (right != null)
                {
                        
                    firstRow.Add(right.SpotID);
                    right = right.rightWall;
                }

                Wall left = wall.Value.leftWall;
                while (true)
                {
                    if (left == null)
                    {
                        break;
                    }
                    else
                    {
                        firstRow.Add(left.SpotID);
                        left = left.leftWall;
                    }

                }
                muros.Add(firstRow);
                firstRow = new List<int>();

            }
        }
        return muros;
    }
}
