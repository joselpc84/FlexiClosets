using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockArrowByOrto : MonoBehaviour
{

	public Button[] button;


	public void CheckBlock ()
	{
		bool Block = Camera.main.orthographic;
		for (int i = 0; i < button.Length; ++i) {
			button [i].interactable = !Block;
		}
	}

}
