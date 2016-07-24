using UnityEngine;
using System.Collections;

public class MakeForm : MonoBehaviour {

	public GameObject Formparent;
	private ChangelingTroop troopscript;

	public void Line() {
		for (int i = 1; i <= Formparent.transform.childCount - 1; i++) {
			//troopscript = Formparent.transform.GetChild (i);
		}
	}
}
