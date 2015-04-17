using UnityEngine;
using System.Collections;

/// <summary>
/// Inidica la posicion del mouse en el grid
/// </summary>
public class ManagerMouseControl : PersistentSingleton<ManagerMouseControl>
{

    public LayerMask Planelayer;


    #region AuxVars:

    RaycastHit hitInfo;
    QuadInfo quad;
    Ray ray;

    #endregion

    public QuadInfo? CurrentMousePos()
    {
        if (quad.index > -1)
            return quad;
        else
            return null;
    }
	
    // Update is called once per frame
    void LateUpdate()
    {
	
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, Planelayer))
        {
		
            ManagerGrid.getCenterNear(hitInfo.point, out quad);
            //  Debug.Log("Posicion Actual Grid: " + quad.index);
        }
        else
        {
            quad.index = -1; 
            quad.center = Vector3.zero;
        }
    }
}
