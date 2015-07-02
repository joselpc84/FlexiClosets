using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;


public class ModuloUI : PersistentSingleton<ModuloUI>
{
    //  public Button wallObject;
    public Button[] objects;

    [HideInInspector]
    public Item currentSelected;

    public CanvasGroup PopUp;

    public Text textColor;

    public string[] MaterialsName;

    public Toggle DefaultMaterial;

    void Start()
    {
        //   EnableObjects(false);
        HidePopUp();

    }

    public void ShowPersonalization(Item prefab)
    {
        currentSelected = prefab;

        if (!(currentSelected is Wall))
        {

            
            currentSelected.dictMaterial.Clear();
            for (int i = 0; i < currentSelected.Materials.Length; ++i)
            {
                currentSelected.dictMaterial.Add(MaterialsName[i], i);
            }

            currentSelected.MaterialIndex = 0;
            Material mat = currentSelected.Materials[currentSelected.MaterialIndex];
            textColor.text = MaterialsName[currentSelected.MaterialIndex];
            currentSelected.SetMaterial(mat);

            DefaultMaterial.isOn = true;

        }
        ShowPopUp();


    }

    public void ChangueMaterial(string mattype)
    {
        if (!(currentSelected is Wall))
        {
            Material mat;

            currentSelected.MaterialIndex = currentSelected.dictMaterial[mattype];
            mat = currentSelected.Materials[currentSelected.MaterialIndex];

            textColor.text = MaterialsName[currentSelected.MaterialIndex];
            currentSelected.SetMaterial(mat);
        }
    }

    public void OnPointerDown()
    {
        ManagerItemDrag.Instance.OnDrag(currentSelected);
        HidePopUp();
    }

    public void OnPointerDown2(Item Item)
    {
        currentSelected = Item;
        OnPointerDown();
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