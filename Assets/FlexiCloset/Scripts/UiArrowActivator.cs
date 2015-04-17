using UnityEngine;
using System.Collections;

public class UiArrowActivator : MonoBehaviour
{

    public BotonGuiWall[] botons;

    public void ActivateBotons()
    {
        for (int i = 0; i < botons.Length; ++i)
        {
            botons[i].CheckIfEnable();
        }
    }

    public void DeActivateBotons()
    {
        for (int i = 0; i < botons.Length; ++i)
        {
            botons[i].gameObject.SetActive(false);
        }
    }
}
