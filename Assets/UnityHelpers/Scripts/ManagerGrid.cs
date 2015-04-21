//
//  Author:
//    Luis Alejandro Vieira lavz24@gmail.com
//
//  Copyright (c) 2014
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct QuadInfo
{

    public int index;
    public Vector3 center;

    public QuadInfo(int ind)
    {

        index = -1;
        center = Vector3.zero;
    }
};

public class Node
{
    public int index = -1;
    public List<int> neighbors = new List<int>();
    public bool isBussy = false;


    public int pathParent = -1;

    public Node(int i, List<int> nei)
    {
	
        index = i;

        neighbors = nei;

    }
};

public class ManagerGrid : Singleton<ManagerGrid>
{
    public PlaneMesh grid;

    public Dictionary<int,Vector3>  infoPosGrid { get; protected set; }

    public Dictionary<int,Node>  infoBFS { get; protected set; }

    public int CountX
    {
	
        get
        {
		
            return grid.sizeX;
        }
    }

    public int CountZ
    {

        get
        {

            return grid.sizeZ;
        }
    }

    public int GridSize
    {
    
        get
        {
            return CountX * CountZ;
        }
    }

    public int Size
    {

        get
        {

            return grid.Size;
        }
    }

    public bool isIndexInRange(int indx)
    {
	
        return indx >= 0 && indx <= ((CountX * CountZ) - 1);
    }

    void Start()
    {


        infoPosGrid = grid.InfoCenters;

        infoBFS = ManagerGrid.createGridInfo(infoPosGrid, grid.sizeX, grid.sizeZ);

    }

    /// <summary>
    /// Gets the center more close to pose in a QuadInfo
    /// This function is inefficient
    /// </summary>
    /// <param name="pos">Position.</param>
    /// <param name="infoQuad">Info quad.</param>
    public static void getCenterNear(Vector3 pos, out QuadInfo infoQuad)
    {


        infoQuad.index = -1;
        infoQuad.center = Vector3.zero;
        foreach (KeyValuePair<int,Vector3> center in ManagerGrid.Instance.infoPosGrid)
        {
            if (infoQuad.index < 0)
            {
                infoQuad.center = center.Value;
                infoQuad.index = center.Key;
            }
            else
            {
                if (Vector3.Distance(center.Value, pos) < Vector3.Distance(infoQuad.center, pos))
                {
                    infoQuad.center = center.Value;
                    infoQuad.index = center.Key;
                }
            }
        }

    }


    public static Dictionary<int,Node> createGridInfo(Dictionary<int,Vector3>  info, int sizeX, int sizeZ)
    {
        Dictionary<int,Node> infoBFS = new Dictionary<int,Node>();
        foreach (KeyValuePair<int,Vector3> i in info)
        {
            List<int> brothers = new List<int>();
//			Debug.Log ("Index: "+i.Key);
            if (i.Key % sizeX == 0)
            {
                brothers.Add(i.Key + 1);//left
                //Debug.Log ("  Nei Left: "+(i.Key+1));
            }
            else
            {
                if ((i.Key + 1) % sizeX == 0)
                {
                    brothers.Add(i.Key - 1);//right
                    //	Debug.Log ("  Nei right: "+(i.Key-1));

                }
                else
                {
                    brothers.Add(i.Key + 1);//left
                    //	Debug.Log ("  Nei Left: "+(i.Key+1));

                    brothers.Add(i.Key - 1);//right
                    //	Debug.Log ("  Nei right: "+(i.Key-1));

                }
            }
            if (i.Key - sizeX >= 0)
            {
                brothers.Add(i.Key - sizeX);//up
                //Debug.Log ("  Nei up: "+(i.Key-sizeX));

            }
            if (i.Key + sizeX <= (sizeX * sizeZ) - 1)
            {
                brothers.Add(i.Key + sizeX);//down
                //Debug.Log ("  Nei down: "+(i.Key+sizeX));

            }

            Node node = new Node(i.Key, brothers);
            infoBFS.Add(i.Key, node);
        }

        return infoBFS;
    }

    public int getIndexUp(int index)
    {
    
        if (index - CountX >= 0)
        {
            return (index - CountX);//up
        }
        return -1;
    }

    public int getIndexDown(int index)
    {

        if (index + CountX <= (CountX * CountZ) - 1)
        {
            return (index + CountX);//down
        }
        return -1;
    }

    public int getIndexLeft(int index)
    {

        if (index % CountX == 0)
        {
            return (index + 1);//left
        }
        else if ((index + 1) % CountX != 0)
        {
            return (index + 1);//left
        }
        return -1;
    }

    public int getIndexRight(int index)
    {

        if (index % CountX != 0)
        {
            if ((index + 1) % CountX == 0)
            {
                return (index - 1);//right
            }
            else
            {
                return (index - 1);//right
            }
        } 
        return -1;
    }
}


