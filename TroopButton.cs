using UnityEngine;
using System.Collections;

public class TroopButton : MonoBehaviour {

	public int order;

	public void clic () {
		SelectedTroopsManager.number = order;
	}
}
