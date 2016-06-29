using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

	 bool isSelecting = false;

	void Start () {
		UnityEngine.Cursor.visible = false;
	}

	void Update () {
		transform.position = Input.mousePosition;
	}
}
