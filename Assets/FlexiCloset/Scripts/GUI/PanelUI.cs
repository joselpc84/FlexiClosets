using UnityEngine;
using System.Collections;

public class PanelUI : MonoBehaviour
{
    public bool active = true;
    protected bool lastActive = true;

    public GameObject[] childs;
    /*
    void Update()
    {
        if (active != lastActive)
        {
            for (int i = 0; i < childs.Length; ++i)
            {
                childs[i].SetActive(active);
            }

            lastActive = active;
        }
        
    }*/
}
