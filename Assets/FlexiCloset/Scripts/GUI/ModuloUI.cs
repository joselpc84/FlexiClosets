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

    public RectTransform PopUp;

    public Text textColor;

    void Start()
    {
        PopUp.gameObject.transform.localScale = Vector3.zero;
        //   EnableObjects(false);

    }

    public void ShowPersonalization(Item prefab)
    {
        currentSelected = prefab;
        textColor.text = currentSelected.mesh[0].sharedMaterial.name;

        PopUp.gameObject.transform.localScale = new Vector3(1, 1, 1);

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
        PopUp.gameObject.transform.localScale = Vector3.zero;

    }

    public void EnableObjects(bool value)
    {
        for (int i = 0; i < objects.Length; ++i)
        {
            objects[i].interactable = value;
        }
    }
}