using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Guarda lso items colocados en la escena
/// </summary>
public class ManagerItemGrid : PersistentSingleton<ManagerItemGrid>
{

	Dictionary<int,Item> items = new Dictionary<int,Item> ();
	Dictionary<TypeForniture,int> itemsTypeCount = new Dictionary<TypeForniture,int> ();

	Dictionary<int,Wall> itemsWall = new Dictionary<int,Wall> ();

	HashSet<Item> allItems = new HashSet<Item> ();

	public Wall wallPrefab;
	public int MaxNumberHeigh = 6;

	public void AddItem (QuadInfo info, Item item, bool isUp = false)
	{
		item.SetQuad (info);
		if (!isUp) {
			items.Add (item.SpotID, item);

			int[] bros = item.SpotBrothersID;
			for (int i = 0; i < bros.Length; ++i) {
				items.Add (bros [i], item);

			}

			if (item is Wall) {
				itemsWall.Add (item.SpotID, item as Wall);

				for (int i = 0; i < bros.Length; ++i) {
					itemsWall.Add (bros [i], item as Wall);

				}
				ModuloUI.Instance.EnableObjects (true);

			}
		}
		allItems.Add (item);
		if (!itemsTypeCount.ContainsKey (item.typeForniture))
			itemsTypeCount.Add (item.typeForniture, 0);
		itemsTypeCount [item.typeForniture] = itemsTypeCount [item.typeForniture] + 1;

		if (itemsTypeCount [item.typeForniture] == 1) {
			//MsotratMensaje
			if (item.MessageTip != null) {
				item.MessageTip.ShowTip ();
			}
		}
		Invoke ("ShowDistanceWall", 0.1f);
	}

	public void RemoveItem (Item item)
	{
		if (!item.isUp) {

			items.Remove (item.SpotID);

			int[] bros = item.SpotBrothersID;
			for (int i = 0; i < bros.Length; ++i) {
				items.Remove (bros [i]);
			}
			if (item is Wall) {
				itemsWall.Remove (item.SpotID);
				for (int i = 0; i < bros.Length; ++i) {
					itemsWall.Remove (bros [i]);
				}
		
			}
		}

		allItems.Remove (item);
		itemsTypeCount [item.typeForniture] -= 1;

		//Apagar y prender de acuerdo si es muro sus distancias
		Invoke ("ShowDistanceWall", 0.1f);
	}

	public void ResetAll ()
	{
		Item[] prueba = new Item[items.Values.Count];
		items.Values.CopyTo (prueba, 0);

		for (int i = 0; i < prueba.Length; ++i) {
			if (prueba [i] != null)
				prueba [i].Remove ();
		}
	}

	void FixedShowDistanceofWall ()
	{
		ShowDistanceWall ();

	}

	public bool isEmptySpot (QuadInfo quad, Item info)
	{
		bool isEmpty = true;
		List<int> posiblePosition = info.getPosibleBrothers (quad.index);
		//Si algn puesto es -1
		for (int i = 0; i < posiblePosition.Count; ++i) {
			if (posiblePosition [i] == -1) {
				isEmpty = false;
				break;
			}
		}
		foreach (var spot in items) {
			if (spot.Key == quad.index) {
                
				isEmpty = false;
			} else {
				for (int i = 0; i < posiblePosition.Count; ++i) {
					if (spot.Key == posiblePosition [i] || posiblePosition [i] == -1) {
						isEmpty = false;
					}
				}
			}
			if (!isEmpty) {
				break;
			}
		}


		return isEmpty;
	}

	Item CheckIsUpIsIn (Item baseI, int index, List<int> posiblePosition)
	{
		Item toReturn = null;

		for (int i = 0; i < baseI.itemUp.Count; ++i) {
			if (baseI.itemUp [i].isInIdex (index)) {
				toReturn = CheckIsUpIsIn (baseI.itemUp [i], index, posiblePosition);

			} else {
				for (int j = 0; j < posiblePosition.Count; ++j) {
					if (baseI.itemUp [i].isInIdex (posiblePosition [j])) {
						toReturn = CheckIsUpIsIn (baseI.itemUp [i], index, posiblePosition);
						break;
					}
				}
			}
			if (toReturn != null)
				break;
		}
		if (toReturn != null)
			return toReturn;
        
		return baseI;
	}

	public Item CanPutUp (QuadInfo quad, Item info)
	{
		bool isNotEmpty = false;
		List<int> posiblePosition = info.getPosibleBrothers (quad.index);
		posiblePosition.Sort ();
		foreach (var spot in items) {
			//Y hay q revisar es con el Maxup no con el hijo.
			//Aqui no necesariamente el key tiene q ser el quad index, y si no lo es ajuro hay q chequear
			//los hermanos y ver si todos coinciden.
			if (spot.Value.itemDown == null && !(spot.Value is Wall)) {
				if (spot.Key == quad.index) {
					
					Item toCheck = null;
					toCheck = CheckIsUpIsIn (spot.Value, quad.index, posiblePosition);

					//Aqui checo q todos los hermanos y la posicion de info esten dentro de spot y q no halla alguien ahi
					List<int> bro = toCheck.getBrothers;
					bro.Sort ();
					for (int i = -1; i < posiblePosition.Count; ++i) {
						isNotEmpty = false;
						for (int j = 0; j < bro.Count; ++j) {
							if (i == -1) {
								if (bro [j] == info.getStartPosition || toCheck.getStartPosition == info.getStartPosition) {
									isNotEmpty = true;
								} else {
									isNotEmpty = false;
								}
							} else {
								if (bro [j] == posiblePosition [i] || toCheck.getStartPosition == posiblePosition [i]) {
									isNotEmpty = true;
								} else {
									isNotEmpty = false;
								}
							}
							if (isNotEmpty)
								break;
						}
						if (!isNotEmpty) {
							//ver si puedo hacer snap to grid
							break;
						}
					}
					if (isNotEmpty) {
						if (toCheck.getMaxHeighNumberPut () + info.HighNumber <= MaxNumberHeigh && toCheck.isTypeForPutUp (info.typeForniture)) {

							return toCheck;
						}

					}
				} else {

					//dejame ver si aqui compruebo q almenos este dentro de el
					if (spot.Value.isInIdex (quad.index)) {
						Item toCheck = null;
						toCheck = CheckIsUpIsIn (spot.Value, quad.index, posiblePosition);
						if (toCheck.getMaxHeighNumberPut () + info.HighNumber <= MaxNumberHeigh && toCheck.isTypeForPutUp (info.typeForniture)) {
							//Aqui tengo q comprobar la rotacion

							return toCheck;
						}
					}
				}
			} 
                
		}
		return null;
	}

	public Wall getWallIfInSpot (QuadInfo quad, Wall info)
	{
    
		if (!isEmptySpot (quad, info)) {
			if (items [quad.index] is Wall)
				return items [quad.index] as Wall;
		}
		return null;

	}

	public List< List<int>> getRowsOfWallUD ()
	{
		List< List<int>> muros = new List< List<int>> ();
		List<int> firstRow = new List<int> ();

		foreach (var wall in itemsWall) {
			bool isInAnyOne = false;
			while (!isInAnyOne) {
				foreach (List<int> listi in muros) {
					if (listi.Contains (wall.Value.SpotID)) {
						isInAnyOne = true;
					}
				}
				break;
			}

			if (!isInAnyOne) {

				firstRow.Add (wall.Value.SpotID);
				Wall up = wall.Value.upWall;
				while (up != null) {

					firstRow.Add (up.SpotID);
					up = up.upWall;
				}

				Wall down = wall.Value.downWall;
				while (true) {
					if (down == null) {
						break;
					} else {
						firstRow.Add (down.SpotID);
						down = down.downWall;
					}

				}
				muros.Add (firstRow);
				firstRow = new List<int> ();

			}
		}
		return muros;
	}

	public List< List<int>> getRowsOfWallLR ()
	{
		List< List<int>> muros = new List< List<int>> ();
		List<int> firstRow = new List<int> ();

		foreach (var wall in itemsWall) {
			bool isInAnyOne = false;
			while (!isInAnyOne) {
				foreach (List<int> listi in muros) {
					if (listi.Contains (wall.Value.SpotID)) {
						isInAnyOne = true;
					}
				}
				break;
			}

			if (!isInAnyOne) {
                
				firstRow.Add (wall.Value.SpotID);
				Wall right = wall.Value.rightWall;
				while (right != null) {
                        
					firstRow.Add (right.SpotID);
					right = right.rightWall;
				}

				Wall left = wall.Value.leftWall;
				while (true) {
					if (left == null) {
						break;
					} else {
						firstRow.Add (left.SpotID);
						left = left.leftWall;
					}

				}
				muros.Add (firstRow);
				firstRow = new List<int> ();

			}
		}
		return muros;
	}

	#region Wall Distance:

	void Start ()
	{
    
		prefabText.CreatePool (10);
		objMeasure.CreatePool (30);
		objMeasurefinal.CreatePool (30);
	}

	public TextMesh prefabText;
	public float TamanoBloques = 0.5f;
	public int minNumOfWall = 2;
	public string unitName = " cm";
	bool _isShowDistanceWall = true;

	public bool isShowDistanceWall {
    
		get {
        
			return _isShowDistanceWall;
		}
		set {
			_isShowDistanceWall = !_isShowDistanceWall;
			if (!_isShowDistanceWall)
				offDistanceWall ();
			else
				ShowDistanceWall ();
		}
	}

	public void offDistanceWall ()
	{
		prefabText.RecycleAll ();
		objMeasure.RecycleAll ();
		objMeasurefinal.RecycleAll ();
	}

	public GameObject objMeasure;
	public GameObject objMeasurefinal;
	public float offSetUp = 1.1f;

	public void ShowDistanceWall ()
	{
		offDistanceWall ();
		if (isShowDistanceWall == false)
			return;
        
		Quaternion rotateAnotherDir = Quaternion.Euler (new Vector3 (0, 90, 0));

		List<Vector3> pos = new List<Vector3> ();
		List<float> valuedistance = new List<float> ();

		List< List<int>> testWallsLR = getRowsOfWallLR ();
		for (int i = 0; i < testWallsLR.Count; i++) {
			if (testWallsLR [i].Count <= minNumOfWall)
				continue;
			Wall right = itemsWall [testWallsLR [i] [0]];
			Wall rightstore = right;
			while (right != null) {
				objMeasure.Spawn (right.transform.position + Vector3.up * offSetUp * right.HighValue);
				rightstore = right;
				right = right.rightWall;
			}
			Wall left = itemsWall [testWallsLR [i] [0]];
			Wall leftstore = left;
			while (left != null) {
				leftstore = left;
				objMeasure.Spawn (left.transform.position + Vector3.up * offSetUp * left.HighValue);
				left = left.leftWall;
			}
			objMeasurefinal.Spawn (leftstore.transform.position + Vector3.up * offSetUp * leftstore.HighValue);
			objMeasurefinal.Spawn (rightstore.transform.position + Vector3.up * offSetUp * rightstore.HighValue);

			valuedistance.Add (testWallsLR [i].Count * TamanoBloques);
			Vector3 dir = leftstore.transform.position - rightstore.transform.position;
			Vector3 poosAux = (rightstore.transform.position + dir.normalized * dir.magnitude * 0.5f) + Vector3.up * rightstore.HighValue;
			pos.Add (poosAux);
		}

		List< List<int>> testWallUD = getRowsOfWallUD ();
		for (int i = 0; i < testWallUD.Count; i++) {
			if (testWallUD [i].Count <= minNumOfWall)
				continue;
			Wall up = itemsWall [testWallUD [i] [0]];
			Wall upstore = up;
			while (up != null) {
				objMeasure.Spawn (up.transform.position + Vector3.up * offSetUp * up.HighValue, rotateAnotherDir);

				upstore = up;
				up = up.upWall;
			}
			Wall down = itemsWall [testWallUD [i] [0]];
			Wall downstore = down;
			while (down != null) {
				objMeasure.Spawn (down.transform.position + Vector3.up * offSetUp * down.HighValue, rotateAnotherDir);

				downstore = down;
				down = down.downWall;
			}
			objMeasurefinal.Spawn (downstore.transform.position + Vector3.up * offSetUp * downstore.HighValue, rotateAnotherDir);
			objMeasurefinal.Spawn (upstore.transform.position + Vector3.up * offSetUp * upstore.HighValue, rotateAnotherDir);

			valuedistance.Add (testWallUD [i].Count * TamanoBloques);
			Vector3 dir = downstore.transform.position - upstore.transform.position;
			Vector3 poosAux = (upstore.transform.position + dir.normalized * dir.magnitude * 0.5f) + Vector3.up * upstore.HighValue;
			pos.Add (poosAux);
		}
		for (int i = 0; i < pos.Count; ++i) {
			TextMesh obj = prefabText.Spawn (pos [i]);
			obj.text = valuedistance [i].ToString () + unitName;
		}
	}

	#endregion

	public bool isShowWalls = true;

	public void ShowWalls ()
	{
		isShowWalls = !isShowWalls;
		if (isShowWalls) {
			foreach (KeyValuePair<int,Wall> pair in itemsWall) {
				pair.Value.SeeWall ();

			}
		} else {
			foreach (KeyValuePair<int,Wall> pair in itemsWall) {
				pair.Value.DontSeeWall ();

			}
		}

	}

	public void GenerateURL ()
	{
		Application.ExternalEval ("window.open('http://ppdesa01.ddns.net/pdftest.php?sku01=75929910025362&sku02=75929910025292','Window title')");
		//=Application.OpenURL ("http://ppdesa01.ddns.net/pdftest.php?sku01=75929910025362&sku02=75929910025292");
	}
}
