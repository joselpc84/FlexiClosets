using UnityEngine;
using System.Collections;

public class EditorForniture : MonoBehaviour
{

	public Mesh[] typeMeshes;
	protected int currentPos = 0;

	public MeshFilter filterM;

	public void LeftMesh ()
	{
		--currentPos;
		if (currentPos < 0)
			currentPos = typeMeshes.Length - 1;
		filterM.mesh = typeMeshes [currentPos];
	}

	public void RigthMesh ()
	{
		++currentPos;
		if (currentPos >= typeMeshes.Length)
			currentPos = 0;
		filterM.mesh = typeMeshes [currentPos];
	}
}
