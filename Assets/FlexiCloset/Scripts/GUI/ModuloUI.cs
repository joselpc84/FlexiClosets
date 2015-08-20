using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;

public enum WallType
{
	Base0 = 0,
	Base1 = 1,
	Base2 = 2,
	EnL = 3,
	Escalonado = 4
}

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


	public CanvasGroup ObjectCanvas;
	public CanvasGroup WallCanvas;

	void Start ()
	{
		//   EnableObjects(false);
		HidePopUp ();

	}

	//Este muestra el personalization de lso objetos
	public void ShowPersonalization (Item prefab)
	{
		currentSelected = prefab;

		if (!(currentSelected is Wall)) {

            
			currentSelected.dictMaterial.Clear ();
			for (int i = 0; i < currentSelected.Materials.Length; ++i) {
				currentSelected.dictMaterial.Add (MaterialsName [i], i);
			}

			currentSelected.MaterialIndex = 0;
			Material mat = currentSelected.Materials [currentSelected.MaterialIndex];
			textColor.text = MaterialsName [currentSelected.MaterialIndex];
			currentSelected.SetMaterial (mat);

			DefaultMaterial.isOn = true;

		}
		ShowPopUp ();
	}


	public Scrollbar barValue1;
	public Scrollbar barValue2;

	public void ShowPersonalizationWall (Item prefab)
	{
		currentSelected = prefab;
		//	wallT = WallType.Base0;
		SetWall (0);
		//numberOfWall_1 = DefaultValue1;
		//barValue1.value = 0;
		//barValue2.value = 0;

		ShowPopUp (false);
	}

	public void ChangueMaterial (string mattype)
	{
		if (!(currentSelected is Wall)) {
			Material mat;

			currentSelected.MaterialIndex = currentSelected.dictMaterial [mattype];
			mat = currentSelected.Materials [currentSelected.MaterialIndex];

			textColor.text = MaterialsName [currentSelected.MaterialIndex];
			currentSelected.SetMaterial (mat);
		}
	}

	//Este hace el drag del check de lso objetos
	public void OnPointerDown ()
	{
		ManagerItemDrag.Instance.OnDrag (currentSelected);
		HidePopUp ();
	}

	public void OnPointerDownWall ()
	{
		int count = 0;

		switch (wallT) {
		case WallType.Base0:
			ManagerItemDrag.Instance.OnDrag (currentSelected);

			break;
		case WallType.Base1:
			for (int i = StartWall1; i >= MaxOfWall1; i += amountWall1) {
				ManagerItemDrag.Instance.OnSpawn (currentSelected, i);
				++count;
				if (count >= numberOfWall_1) {
					break;
				}
			}
			break;
		case WallType.Base2:
			for (int i = StartWall2; i <= MaxOfWall2; i += amountWall2) {
				ManagerItemDrag.Instance.OnSpawn (currentSelected, i);
				++count;
				if (count >= numberOfWall_1) {
					break;
				}
			}
			break;
		case WallType.EnL:
			ManagerItemDrag.Instance.OnSpawn (currentSelected, 0);
			count = 1;
			for (int i = (amountWall1 * -1); i <= StartWall1; i += (amountWall1 * -1)) {
				ManagerItemDrag.Instance.OnSpawn (currentSelected, i);
				++count;
				if (count >= numberOfWall_1) {
					break;
				}
			}
			count = 1;
			for (int i = amountWall2; i <= MaxOfWall2; i += amountWall2) {
				ManagerItemDrag.Instance.OnSpawn (currentSelected, i);
				++count;
				if (count >= numberOfWall_2) {
					break;
				}
			}
			break;
		case WallType.Escalonado:
			break;
		}

		HidePopUp ();
	}

	public void EnableObjects (bool value)
	{
		for (int i = 0; i < objects.Length; ++i) {
			objects [i].interactable = value;
		}
	}

	public void ShowPopUp (bool isObject = true)
	{
		// PopUp.gameObject.transform.localScale = new Vector3(1, 1, 1);
		PopUp.alpha = 1;
		PopUp.blocksRaycasts = true;
		PopUp.interactable = true;
		if (isObject) {

			WallCanvas.alpha = 0;
			WallCanvas.blocksRaycasts = false;
			WallCanvas.interactable = false;

			ObjectCanvas.alpha = 1;
			ObjectCanvas.blocksRaycasts = true;
			ObjectCanvas.interactable = true;
		} else {

			ObjectCanvas.alpha = 0;
			ObjectCanvas.blocksRaycasts = false;
			ObjectCanvas.interactable = false;

			WallCanvas.alpha = 1;
			WallCanvas.blocksRaycasts = true;
			WallCanvas.interactable = true;
		}
	}

	public void HidePopUp ()
	{
		//PopUp.gameObject.transform.localScale = Vector3.zero;
		PopUp.alpha = 0;
		PopUp.blocksRaycasts = false;
		PopUp.interactable = false;

		ObjectCanvas.alpha = 0;
		ObjectCanvas.blocksRaycasts = false;
		ObjectCanvas.interactable = false;

		WallCanvas.alpha = 0;
		WallCanvas.blocksRaycasts = false;
		WallCanvas.interactable = false;
	}


	#region WallLogic:

	[Header ("Wall")]
	public WallType wallT;
	public int DefaultValue1 = 1;
	public int MaxGrid = 50;

	public int StartWall1 = 49;
	public int MaxOfWall1 = 0;
	public int amountWall1 = -1;

	public int StartWall2 = 0;
	public int MaxOfWall2 = 2450;
	public int amountWall2 = 50;

	int numberOfWall_1 = 1;
	int numberOfWall_2 = 1;

	public Text sliderResult_1;
	public Text sliderMin_1;
	public Text sliderMax_1;
	public Text sliderResult_2;
	public Text sliderMin_2;
	public Text sliderMax_2;

	public void SetWall (int type)
	{



		//	wallT = type;
		switch (type) {
		case 0:
			wallT = WallType.Base0;
			break;
		case 1:
			wallT = WallType.Base1;
			break;
		case 2:
			wallT = WallType.Base2;
			break;
		case 3:
			wallT = WallType.EnL;
			break;
		case 4:
			wallT = WallType.Escalonado;
			break;
		default:
			Debug.LogError ("Asignar mejor el numero");
			break;
		}
		numberOfWall_1 = 1;
		numberOfWall_2 = 1;
		barValue1.value = 0;
		barValue2.value = 0;

		sliderResult_1.text = numberOfWall_1.ToString ();
		sliderMin_1.text = "1";
		sliderMax_1.text = "50";
		sliderResult_2.text = numberOfWall_2.ToString ();
		sliderMin_2.text = "1";
		sliderMax_2.text = "50";
	}


	public float SetValueWall1 {
		set {
			switch (wallT) {
			case WallType.Base0:
				break;
			case WallType.Base1:
				numberOfWall_1 = (int)((MaxGrid - 1) * value + 1);
				break;
			case WallType.Base2:
				numberOfWall_1 = (int)((MaxGrid - 1) * value + 1);
				break;
			case WallType.EnL:
				numberOfWall_1 = (int)((MaxGrid - 1) * value + 1);
				break;
			case WallType.Escalonado:
				numberOfWall_1 = (int)((MaxGrid - 1) * value + 1);
				break;
			}

			sliderResult_1.text = numberOfWall_1.ToString ();
		}

	}

	public float SetValueWall2 {
		set {
			switch (wallT) {
			case WallType.Base0:
				break;
			case WallType.Base1:
				break;
			case WallType.Base2:
				break;
			case WallType.EnL:
				numberOfWall_2 = (int)((MaxGrid - 1) * value + 1);
				break;
			case WallType.Escalonado:
				numberOfWall_2 = (int)((MaxGrid - 1) * value + 1);
				break;
			}
			sliderResult_2.text = numberOfWall_2.ToString ();

		}
	}
	// de 1 en 1 [49,0] y de 50 en 50 [0,2450]

	#endregion
}