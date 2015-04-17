using UnityEngine;
using System.Collections;

public class Wall : Item
{

    public UiArrowActivator GUI;

    protected override void LogicOnClicked()
    {
        GUI.ActivateBotons();
    }


    protected override void LogicOnCancel()
    {
        GUI.DeActivateBotons();

    }

}
