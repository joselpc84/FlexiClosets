using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeMaterialSelected : MonoBehaviour
{

    public Image img;

    public Sprite NonSelected;
    public Sprite Selected;

    public void OnSelect(bool value)
    {
        if (value)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    public void Select()
    {
        img.sprite = Selected;
    }

    public void Deselect()
    {
        img.sprite = NonSelected;
    }

}
