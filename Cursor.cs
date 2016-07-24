using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cursor : MonoBehaviour {
	
	private RaycastHit hitDUDE;
	public Sprite Normal;
	public Sprite Gather;
	private Image img;

	void Start () {
		UnityEngine.Cursor.visible = false;

		img = gameObject.GetComponent<Image> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			UnityEngine.Cursor.visible = true;
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		Physics.Raycast (ray, out hitDUDE);

		if (hitDUDE.collider.tag == "Terra")
			img.sprite = Normal;
		else 
			if (SelectedTroops.ChangelingsNB >= 1)
				img.sprite = Gather;

		transform.position = Input.mousePosition;
	}
}
