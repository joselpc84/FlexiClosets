using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;

public class ModuloUI : PersistentSingleton<ModuloUI>
{
    //  public Button wallObject;
    public Button[] objects;

    [HideInInspector]
    public Item currentSelected;

    public CanvasGroup PopUp;

    public Text textColor;

    void Start()
    {
        //   EnableObjects(false);
        HidePopUp();
    }

    public void ShowPersonalization(Item prefab)
    {
        currentSelected = prefab;
        textColor.text = currentSelected.mesh[0].sharedMaterial.name;
        ShowPopUp();

    }

    public void ChangueMaterial(Material mat)
    {
        if (!(currentSelected is Wall))
        {
            //CAmbiando Material
            Debug.Log("Cambiando: " + mat);
            textColor.text = mat.name;
            currentSelected.SetMaterial(mat);
        }
    }

    public void OnPointerDown()
    {
        ManagerItemDrag.Instance.OnDrag(currentSelected);
        HidePopUp();
    }

    public void EnableObjects(bool value)
    {
        for (int i = 0; i < objects.Length; ++i)
        {
            objects[i].interactable = value;
        }
    }

    public void ShowPopUp()
    {
        // PopUp.gameObject.transform.localScale = new Vector3(1, 1, 1);
        PopUp.alpha = 1;
        PopUp.blocksRaycasts = true;
        PopUp.interactable = true;
    }

    public void HidePopUp()
    {
        //PopUp.gameObject.transform.localScale = Vector3.zero;
        PopUp.alpha = 0;
        PopUp.blocksRaycasts = false;
        PopUp.interactable = false;
    }
}