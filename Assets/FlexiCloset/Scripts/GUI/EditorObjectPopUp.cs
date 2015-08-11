using UnityEngine;
using System.Collections;

public class EditorObjectPopUp : PersistentSingleton<EditorObjectPopUp>
{


	public void ChangueMaterial (string mattype)
	{
		if (!(GUI_ItemController.Instance.item is Wall)) {
			GUI_ItemController.Instance.item.DeSelectedMesh ();
			GUI_ItemController.Instance.item.MaterialIndex = ModuloUI.Instance.currentSelected.dictMaterial [mattype];
			GUI_ItemController.Instance.item.SelectedMesh ();
		}
	}
}
