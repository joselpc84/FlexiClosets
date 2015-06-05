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
                ModuloUI.Instance.EnableObjects(true);
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

    Item CheckIsUpIsIn(Item baseI, int index)
    {
        Item toReturn = null;

        for (int i = 0; i < baseI.itemUp.Count; ++i)
        {
            if (baseI.itemUp[i].isInIdex(index))
            {
                toReturn = CheckIsUpIsIn(baseI.itemUp[i], index);

            }
            if (toReturn != null)
                break;
        }
        if (toReturn != null)
            return toReturn;
        
        return baseI;
    }

    public Item CanPutUp(QuadInfo quad, Item info)
    {
        bool isNotEmpty = false;
        List<int> posiblePosition = info.getPosibleBrothers(quad.index);
        posiblePosition.Sort();
        foreach (var spot in items)
        {
            //Y hay q revisar es con el Maxup no con el hijo.
            //Aqui no necesariamente el key tiene q ser el quad index, y si no lo es ajuro hay q chequear
            //los hermanos y ver si todos coinciden.
            if (spot.Key == quad.index && spot.Value.itemDown == null)
            {
                Item toCheck = null;
                toCheck = CheckIsUpIsIn(spot.Value, quad.index);

                //Aqui checo q todos los hermanos y la posicion de info esten dentro de spot y q no halla alguien ahi
                List<int> bro = toCheck.getBrothers;
                bro.Sort();
                for (int i = -1; i < posiblePosition.Count; ++i)
                {
                    isNotEmpty = false;
                    for (int j = 0; j < bro.Count; ++j)
                    {
                        if (i == -1)
                        {
                            if (bro[j] == info.getStartPosition || toCheck.getStartPosition == info.getStartPosition)
                            {
                                isNotEmpty = true;
                            }
                            else
                            {
                                isNotEmpty = false;
                            }
                        }
                        else
                        {
                            if (bro[j] == posiblePosition[i] || toCheck.getStartPosition == posiblePosition[i])
                            {
                                isNotEmpty = true;
                            }
                            else
                            {
                                isNotEmpty = false;
                            }
                        }
                        if (isNotEmpty)
                            break;
                    }
                    if (!isNotEmpty)
                        break;
                }
                if (isNotEmpty)
                {
                    if (toCheck.getMaxHeighNumberPut() + info.HighNumber <= MaxNumberHeigh && toCheck.isTypeForPutUp(info.typeForniture))
                    {

                        return toCheck;
                    }

                }

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
