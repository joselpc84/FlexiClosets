//
// PlaneMesh.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaneMesh : MonoBehaviour
{
	public Material material;

	public int sizeX = 5;
	public int sizeZ = 5;
	public int Size = 10;

	public GridInfo Info { get; protected set;}

	public Dictionary<int,Vector3> InfoCenters{
	
		get{
		
			return Info.centers;
		}
	}

	protected MeshRenderer mesh;	
	public void setMaterial(Material mat){

		mesh.material = mat;
	}
	void Awake()
	{
		//testrt2.CreatePool();
		Info =  MeshCreator.CreatePlaneXZwithUV(transform, Size, sizeX, sizeZ);

		BoxCollider coll = gameObject.AddComponent<BoxCollider>();
		coll.size = new Vector3(sizeX, 1, sizeZ) * Size;
		coll.center = new Vector3(0,-Size*0.5f,0);
		MeshFilter meshi = gameObject.AddComponent<MeshFilter>();
		meshi.mesh = Info.mesh;

		material.SetFloat("_NumX", sizeX);
		material.SetFloat("_NumZ", sizeZ);


		mesh = gameObject.AddComponent<MeshRenderer>();
		mesh.material = material;

	}


}


