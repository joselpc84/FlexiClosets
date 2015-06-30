using UnityEngine;
using System.Collections;

public class UiArrowActivator_Item : UiArrowActivator
{
    public MeshRenderer[] renderes;

    public override void ActivateBotons()
    {
        for (int i = 0; i < botons.Length; ++i)
        {
            botons[i].enabled = true;
            renderes[i].enabled = true;
        }


    }

    public override void DeActivateBotons()
    {
        for (int i = 0; i < botons.Length; ++i)
        {
            
            botons[i].enabled = false;
            renderes[i].enabled = false;
        }

    }
}
