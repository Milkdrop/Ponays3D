using UnityEngine;
using System.Collections;

public class SelectionLines : MonoBehaviour {

	public bool validated;
	public bool first = true;
	public Texture texture;
	private Rect rect;

	void Update () {
		if (Input.GetMouseButton (0)) {
			if (first) {
				validated = true;
				rect.xMin = Input.mousePosition.x;
				rect.yMin = Screen.height - Input.mousePosition.y;
				first = false;
			}
		} else {
			validated = false;
			first = true;
		}
	}

	void OnGUI() {
		if (validated) {
			rect.xMax = Input.mousePosition.x;
			rect.yMax = Screen.height - Input.mousePosition.y;
			GUI.DrawTexture (rect, texture);
		}
	}
}
