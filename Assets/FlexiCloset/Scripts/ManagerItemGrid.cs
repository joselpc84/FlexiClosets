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

    HashSet<Item> allItems = new HashSet<Item>();

    public Wall wallPrefab;
    public int MaxNumberHeigh = 6;

    public void AddItem(QuadInfo info, Item item, bool isUp = false)
    {
        item.SetQuad(info);
        if (!isUp)
        {
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
        allItems.Add(item);

    }

    public void RemoveItem(Item item)
    {
        if (!item.isUp)
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

        allItems.Remove(item);
    }

    public bool isEmptySpot(QuadInfo quad, Item info)
    {
        bool isEmpty = true;
        List<int> posiblePosition = info.getPosibleBrothers(quad.index);
        //Si algn puesto es -1
        for (int i = 0; i < posiblePosition.Count; ++i)
        {
            if (posiblePosition[i] == -1)
            {
                isEmpty = false;
                break;
            }
        }
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
                    if (spot.Key == posiblePosition[i] || posiblePosition[i] == -1)
                    {
                        isEmpty = false;
                    }
                }
            }
            if (!isEmpty)
            {
                break;
            }
        }


        return isEmpty;
    }

    public Item CanPutUp(QuadInfo quad, Item info)
    {
        Item baseItem = null;
        bool isNotEmpty = false;
        List<int> posiblePosition = info.getPosibleBrothers(quad.index);
        posiblePosition.Sort();
        int currentHeigh = 0;
        bool canPutUp = false;
        foreach (var spot in items)
        {
            Item MaxUp = spot.Value.itemUp;
            bool haveUp = MaxUp != null;
            currentHeigh = spot.Value.HighNumber;
            canPutUp = spot.Value.isTypeForPutUp(info.typeForniture);
            if (haveUp)
            {
                while (MaxUp != null)
                {
                    currentHeigh += MaxUp.HighNumber;
                    canPutUp = MaxUp.isTypeForPutUp(info.typeForniture);
                    MaxUp = MaxUp.itemUp;
                }
            }
            if (spot.Key == quad.index && spot.Value.getStartPosition == quad.index)
            {
                baseItem = spot.Value;
                isNotEmpty = true;
            } 
            if (isNotEmpty)
            {
                List<int> bro = spot.Value.getBrothers;
                bro.Sort();
                if (bro.Count == posiblePosition.Count)
                {
                    for (int i = 0; i < bro.Count && isNotEmpty; ++i)
                    {
                        if (bro[i] == posiblePosition[i])
                        {
                            isNotEmpty = true;
                        }
                    }
                }
                else
                {
                    isNotEmpty = false;
                }
                if (isNotEmpty)
                    break;
            }
        }
        if (isNotEmpty)
        {
            //Aqui checo que este entre los ti[ops que pued eir arriba
            if (currentHeigh + info.HighNumber <= MaxNumberHeigh && canPutUp)
            {
                
                return baseItem;
            }
                
        }
        return null;
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
